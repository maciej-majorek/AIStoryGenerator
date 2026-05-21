/**
 * Formatting utilities for different story formats
 */

export interface FormatOptions {
  format: 'Prose' | 'Screenplay' | 'Stageplay' | 'Poem';
  content: string;
}

/**
 * Format content based on story format type
 */
export const formatStory = ({ format, content }: FormatOptions): string => {
  switch (format) {
    case 'Prose':
      return formatProse(content);
    case 'Screenplay':
      return formatScreenplay(content);
    case 'Stageplay':
      return formatStageplay(content);
    case 'Poem':
      return formatPoem(content);
    default:
      return content;
  }
};

/**
 * Format content as prose (paragraph-based)
 */
export const formatProse = (content: string): string => {
  return content
    .split('\n')
    .filter((line) => line.trim())
    .map((line) => line.trim())
    .join('\n\n');
};

/**
 * Format content as screenplay (scene headers, action, dialogue)
 */
export const formatScreenplay = (content: string): string => {
  const lines = content.split('\n');
  return lines
    .map((line) => {
      const trimmed = line.trim();
      if (trimmed.match(/^(INT\.|EXT\.|INT\/EXT\.)/)) {
        return `${trimmed}`; // Scene header
      }
      if (trimmed.match(/^[A-Z ]+$/) && trimmed.length > 5) {
        return `${trimmed}`; // Character name
      }
      if (trimmed.startsWith('(') && trimmed.endsWith(')')) {
        return `  ${trimmed}`; // Parenthetical
      }
      return trimmed; // Dialog or action
    })
    .join('\n');
};

/**
 * Format content as stageplay (acts, scenes, dialogue)
 */
export const formatStageplay = (content: string): string => {
  const lines = content.split('\n');
  return lines
    .map((line) => {
      const trimmed = line.trim();
      if (trimmed.match(/^ACT \d+/i)) {
        return `\n${trimmed}\n${'='.repeat(trimmed.length)}`;
      }
      if (trimmed.match(/^SCENE \d+/i)) {
        return `\n${trimmed}\n${'-'.repeat(trimmed.length)}`;
      }
      return trimmed;
    })
    .join('\n');
};

/**
 * Format content as poem (preserve line breaks)
 */
export const formatPoem = (content: string): string => {
  return content.split('\n').map((line) => line.trim()).join('\n');
};

/**
 * Strip formatting markers from content
 */
export const stripFormatting = (content: string): string => {
  return content
    .replace(/\[.*?\]/g, '') // Remove bracketed annotations
    .replace(/\{.*?\}/g, '') // Remove braced annotations
    .trim();
};
