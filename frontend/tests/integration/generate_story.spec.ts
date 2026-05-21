import { test, expect } from '@playwright/test';

test.describe('Story Generation E2E Tests', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5173');
  });

  test('should render the app with form and viewer', async ({ page }) => {
    // Check header
    const header = page.locator('h1:text("AI Story Generator")');
    await expect(header).toBeVisible();

    // Check form exists
    const form = page.locator('[role="form"]');
    await expect(form).toBeVisible();

    // Check viewer exists
    const viewer = page.locator('[role="region"]:has-text("Generated story")');
    await expect(viewer).toBeVisible();
  });

  test('should fill form and generate story', async ({ page }) => {
    // Fill in the form
    await page.fill('textarea[name="plot"]', 'A hero must save the kingdom');
    await page.fill('textarea[name="characters"]', 'King Arthur and Merlin');
    await page.fill('textarea[name="setting"]', 'Medieval Britain');
    await page.fill('input[name="genre"]', 'Fantasy');
    await page.fill('input[name="theme"]', 'Courage');

    // Select format
    await page.selectOption('select[name="format"]', 'Prose');

    // Set length
    await page.fill('input[name="length"]', '300');

    // Submit form
    const generateButton = page.locator('button:has-text("Generate Story")');
    await generateButton.click();

    // Wait for story to appear (with timeout for API response)
    const storyContent = page.locator('.story-header');
    await expect(storyContent).toBeVisible({ timeout: 15000 });

    // Verify story metadata is displayed
    const metadata = page.locator('.story-metadata');
    await expect(metadata).toBeVisible();
  });

  test('should display error on form validation', async ({ page }) => {
    // Try to submit empty form
    const generateButton = page.locator('button:has-text("Generate Story")');
    
    // HTML5 validation should prevent submission
    const plotInput = page.locator('textarea[name="plot"]');
    const isRequired = await plotInput.getAttribute('required');
    
    expect(isRequired).toBe('');
  });

  test('should preserve form state', async ({ page }) => {
    // Fill in form
    const plotText = 'A mysterious island';
    await page.fill('textarea[name="plot"]', plotText);

    // Verify the text was entered
    const plotInput = page.locator('textarea[name="plot"]');
    await expect(plotInput).toHaveValue(plotText);
  });

  test('should display loading state during generation', async ({ page }) => {
    // Fill minimal form
    await page.fill('textarea[name="plot"]', 'Test plot');
    await page.fill('textarea[name="characters"]', 'Test character');
    await page.fill('textarea[name="setting"]', 'Test setting');

    // Click generate
    const generateButton = page.locator('button:has-text("Generate Story")');
    await generateButton.click();

    // Check for loading indicator
    const loadingText = page.locator('h2:has-text("Generating")');
    await expect(loadingText).toBeVisible({ timeout: 2000 });
  });

  test('should display different story formats correctly', async ({ page }) => {
    // Fill form
    await page.fill('textarea[name="plot"]', 'Test plot');
    await page.fill('textarea[name="characters"]', 'Test character');
    await page.fill('textarea[name="setting"]', 'Test setting');

    // Test Prose format
    await page.selectOption('select[name="format"]', 'Prose');
    let generateButton = page.locator('button:has-text("Generate Story")');
    await generateButton.click();
    
    await expect(page.locator('.format-badge')).toContainText('Prose', { timeout: 15000 });

    // Test Screenplay format
    await page.selectOption('select[name="format"]', 'Screenplay');
    generateButton = page.locator('button:has-text("Generate Story")');
    await generateButton.click();
    
    await expect(page.locator('.format-badge')).toContainText('Screenplay', { timeout: 15000 });
  });

  test('should have accessible form controls', async ({ page }) => {
    // Verify all form inputs have labels or aria-labels
    const inputs = page.locator('input, textarea, select');
    const count = await inputs.count();

    for (let i = 0; i < count; i++) {
      const input = inputs.nth(i);
      const ariaLabel = await input.getAttribute('aria-label');
      const id = await input.getAttribute('id');
      
      if (id) {
        const label = page.locator(`label[for="${id}"]`);
        const labelCount = await label.count();
        const hasLabel = labelCount > 0;
        expect(ariaLabel || hasLabel).toBeTruthy();
      }
    }
  });
});
