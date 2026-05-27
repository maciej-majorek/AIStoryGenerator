import React from 'react';
import '../styles/export.css';

interface ExportControlsProps {
  story: string;
  format: string;
  onCopySuccess?: () => void;
  onCopyError?: (error: Error) => void;
}

export const ExportControls: React.FC<ExportControlsProps> = ({
  story,
  format,
  onCopySuccess,
  onCopyError,
}) => {
  const handleCopyToClipboard = async () => {
    try {
      await navigator.clipboard.writeText(story);
      onCopySuccess?.();
    } catch (error) {
      onCopyError?.(error instanceof Error ? error : new Error('Failed to copy'));
    }
  };

  const handleDownload = () => {
    try {
      const element = document.createElement('a');
      const file = new Blob([story], { type: 'text/plain' });
      element.href = URL.createObjectURL(file);
      element.download = `story-${new Date().toISOString().split('T')[0]}.txt`;
      document.body.appendChild(element);
      element.click();
      document.body.removeChild(element);
      onCopySuccess?.();
    } catch (error) {
      onCopyError?.(error instanceof Error ? error : new Error('Failed to download'));
    }
  };

  const handleCopyMetadata = async () => {
    try {
      const metadata = `Format: ${format}\nLength: ${story.length} characters\nGenerated: ${new Date().toLocaleString()}`;
      await navigator.clipboard.writeText(metadata);
      onCopySuccess?.();
    } catch (error) {
      onCopyError?.(error instanceof Error ? error : new Error('Failed to copy metadata'));
    }
  };

  return (
    <div className="export-controls" role="group" aria-label="Story export options">
      <button
        className="export-btn copy-btn"
        onClick={handleCopyToClipboard}
        title="Copy the full story to clipboard"
        aria-label="Copy story to clipboard"
      >
        📋 Copy Story
      </button>

      <button
        className="export-btn download-btn"
        onClick={handleDownload}
        title="Download the story as a text file"
        aria-label="Download story as text file"
      >
        ⬇️ Download
      </button>

      <button
        className="export-btn metadata-btn"
        onClick={handleCopyMetadata}
        title="Copy story metadata"
        aria-label="Copy story metadata"
      >
        ℹ️ Copy Info
      </button>
    </div>
  );
};

export default ExportControls;
