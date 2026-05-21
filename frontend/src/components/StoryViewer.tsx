import React from 'react';
import { GeneratedStory } from '../services/api';
import ExportControls from './ExportControls';
import '../styles/viewer.css';

interface StoryViewerProps {
  story: GeneratedStory | null;
  isLoading?: boolean;
  error?: string | null;
}

export const StoryViewer: React.FC<StoryViewerProps> = ({ story, isLoading = false, error = null }) => {
  const [copyNotification, setCopyNotification] = React.useState<string | null>(null);

  const handleCopySuccess = () => {
    setCopyNotification('Copied!');
    setTimeout(() => setCopyNotification(null), 2000);
  };

  const handleCopyError = (error: Error) => {
    setCopyNotification(`Error: ${error.message}`);
    setTimeout(() => setCopyNotification(null), 3000);
  };

  if (error) {
    return (
      <div className="story-viewer error" role="alert">
        <h2>Error</h2>
        <p>{error}</p>
      </div>
    );
  }

  if (isLoading) {
    return (
      <div className="story-viewer loading" role="status" aria-live="polite">
        <h2>Generating your story...</h2>
        <div className="spinner" aria-hidden="true" />
        <p>This may take a few moments.</p>
      </div>
    );
  }

  if (!story) {
    return (
      <div className="story-viewer empty">
        <h2>Your Story</h2>
        <p>Fill out the form and generate a story to see it here.</p>
      </div>
    );
  }

  const renderContent = () => {
    switch (story.format.toLowerCase()) {
      case 'screenplay':
        return (
          <pre className="story-content screenplay">
            {story.content}
          </pre>
        );
      case 'stageplay':
        return (
          <pre className="story-content stageplay">
            {story.content}
          </pre>
        );
      case 'poem':
        return (
          <div className="story-content poem">
            {story.content.split('\n').map((line, index) => (
              <p key={index}>{line}</p>
            ))}
          </div>
        );
      case 'prose':
      default:
        return (
          <div className="story-content prose">
            {story.content.split('\n\n').map((paragraph, index) => (
              <p key={index}>{paragraph}</p>
            ))}
          </div>
        );
    }
  };

  return (
    <article className="story-viewer">
      <div className="story-header">
        <h2>Generated Story</h2>
        <div className="story-metadata">
          <span className="format-badge">{story.format}</span>
          <span className="length-info">{story.length} characters</span>
          {story.tokensUsed > 0 && <span className="tokens-info">{story.tokensUsed} tokens</span>}
        </div>
      </div>

      {story.safetyFlags && story.safetyFlags.length > 0 && (
        <div className="safety-notice" role="alert">
          <strong>Safety Notice:</strong> {story.safetyFlags.join(', ')}
        </div>
      )}

      <div className="story-body">
        {renderContent()}
      </div>

      <ExportControls
        story={story.content}
        format={story.format}
        onCopySuccess={handleCopySuccess}
        onCopyError={handleCopyError}
      />

      {copyNotification && (
        <div className="notification" role="status">
          {copyNotification}
        </div>
      )}

      <div className="story-footer">
        <small>Generated on {new Date(story.createdAt).toLocaleString()}</small>
      </div>
    </article>
  );
};

export default StoryViewer;
