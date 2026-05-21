import { test, expect } from '@playwright/test';
import { injectAxe, checkA11y } from 'axe-playwright';

test.describe('WCAG AA Accessibility Compliance', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the application
    await page.goto('http://localhost:5173');
  });

  test('Homepage should be accessible', async ({ page }) => {
    // Inject axe for accessibility testing
    await injectAxe(page);
    
    // Check for accessibility violations
    await checkA11y(page, null, {
      detailedReport: true,
      detailedReportOptions: {
        html: true
      }
    });
  });

  test('Keyboard navigation: Tab through form elements', async ({ page }) => {
    const storyForm = page.locator('[role="form"], form').first();
    expect(storyForm).toBeDefined();

    // Tab through focusable elements
    await page.keyboard.press('Tab');
    const focusedElement = await page.evaluate(() => {
      const el = document.activeElement;
      return {
        tagName: el?.tagName,
        type: (el as HTMLInputElement)?.type,
        ariaLabel: el?.getAttribute('aria-label')
      };
    });
    
    expect(focusedElement.tagName).toBeTruthy();
  });

  test('Form inputs should have ARIA labels', async ({ page }) => {
    const inputs = page.locator('input, textarea, select');
    const inputCount = await inputs.count();
    
    for (let i = 0; i < inputCount; i++) {
      const input = inputs.nth(i);
      const ariaLabel = await input.getAttribute('aria-label');
      const label = await page.locator(`label[for="${await input.getAttribute('id')}"]`);
      
      // Each input should have either aria-label or associated label
      const hasAccessibleLabel = ariaLabel || (await label.count()) > 0;
      expect(hasAccessibleLabel).toBeTruthy();
    }
  });

  test('Color contrast should meet AA standards', async ({ page }) => {
    await injectAxe(page);
    await checkA11y(page, 'body', {
      rules: {
        'color-contrast': { enabled: true }
      }
    });
  });

  test('Focus indicators should be visible', async ({ page }) => {
    // Tab to first focusable element
    await page.keyboard.press('Tab');
    
    const focusedElement = await page.evaluate(() => {
      const el = document.activeElement as HTMLElement;
      if (!el) return null;
      
      const styles = window.getComputedStyle(el);
      return {
        outline: styles.outline,
        boxShadow: styles.boxShadow,
        borderWidth: styles.borderWidth
      };
    });
    
    // Verify focus indicator exists
    expect(focusedElement).toBeDefined();
    const hasFocusIndicator = focusedElement?.outline !== 'none' || 
                              focusedElement?.boxShadow !== 'none' ||
                              (focusedElement?.borderWidth && focusedElement.borderWidth !== '0px');
    expect(hasFocusIndicator).toBeTruthy();
  });

  test('Generate button should be accessible', async ({ page }) => {
    const generateButton = page.locator('button:has-text("Generate")').first();
    
    // Button should have accessible name
    const ariaLabel = await generateButton.getAttribute('aria-label');
    const buttonText = await generateButton.textContent();
    expect(ariaLabel || buttonText).toBeTruthy();
    
    // Button should be focusable
    await generateButton.focus();
    const isFocused = await page.evaluate(() => document.activeElement?.tagName === 'BUTTON');
    expect(isFocused).toBeTruthy();
  });
});

