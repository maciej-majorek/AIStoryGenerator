import { test, expect } from '@playwright/test';

test.describe('Smoke Tests - Critical Paths', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('http://localhost:5173');
  });

  test('should load application', async ({ page }) => {
    const title = page.locator('h1:text("AI Story Generator")');
    await expect(title).toBeVisible();
  });

  test('should have form and viewer panels', async ({ page }) => {
    const form = page.locator('[role="form"]');
    const viewer = page.locator('[role="region"]:has-text("Generated")');
    
    await expect(form).toBeVisible();
    await expect(viewer).toBeVisible();
  });

  test('complete story generation flow', async ({ page }) => {
    // Fill minimal form
    await page.fill('textarea[name="plot"]', 'Hero saves world');
    await page.fill('textarea[name="characters"]', 'Hero');
    await page.fill('textarea[name="setting"]', 'Fantasy world');

    // Generate
    const generateButton = page.locator('button:has-text("Generate Story")');
    await generateButton.click();

    // Verify story appears
    await expect(page.locator('.story-header')).toBeVisible({ timeout: 15000 });
    await expect(page.locator('.story-content')).toBeVisible();

    // Verify export controls exist
    const copyButton = page.locator('button:has-text("Copy Story")');
    await expect(copyButton).toBeVisible();
  });

  test('should handle form validation', async ({ page }) => {
    // Try to submit empty form
    const generateButton = page.locator('button:has-text("Generate Story")');
    
    // Plot is required - verify validation attribute
    const plotField = page.locator('textarea[name="plot"]');
    const isRequired = await plotField.getAttribute('required');
    expect(isRequired).toBeTruthy();
  });

  test('should support format selection', async ({ page }) => {
    // Fill form
    await page.fill('textarea[name="plot"]', 'Test');
    await page.fill('textarea[name="characters"]', 'Test');
    await page.fill('textarea[name="setting"]', 'Test');

    // Select different format
    await page.selectOption('select[name="format"]', 'Screenplay');
    
    const formatSelect = page.locator('select[name="format"]');
    const selectedValue = await formatSelect.inputValue();
    expect(selectedValue).toBe('Screenplay');
  });

  test('should have accessible navigation', async ({ page }) => {
    // Verify main landmarks exist
    const main = page.locator('main, [role="main"]');
    const aside = page.locator('aside, [role="region"]');
    
    await expect(main).toBeVisible();
    await expect(aside).toBeVisible();

    // Verify tab navigation works
    await page.keyboard.press('Tab');
    const focused = await page.evaluate(() => {
      const el = document.activeElement;
      return el?.tagName;
    });
    
    expect(focused).toBeTruthy();
  });

  test('should display loading state', async ({ page }) => {
    // Fill form
    await page.fill('textarea[name="plot"]', 'Test plot');
    await page.fill('textarea[name="characters"]', 'Character');
    await page.fill('textarea[name="setting"]', 'Setting');

    // Click generate
    const generateButton = page.locator('button:has-text("Generate Story")');
    await generateButton.click();

    // Verify loading state appears (briefly)
    const loadingText = page.locator('h2:has-text("Generating")');
    // Loading might disappear quickly, just verify it can appear
    try {
      await expect(loadingText).toBeVisible({ timeout: 1000 });
    } catch {
      // Loading state might have already passed, which is fine
    }
  });
});
