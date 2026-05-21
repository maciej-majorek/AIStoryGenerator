import React, { useState } from 'react';
import StoryForm from './components/StoryForm';
import StoryViewer from './components/StoryViewer';
import GenerationApiClient, { StoryRequest, GeneratedStory } from './services/api';
import './styles.css';

export default function App() {
  const [story, setStory] = useState<GeneratedStory | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleGenerateStory = async (request: StoryRequest) => {
    setIsLoading(true);
    setError(null);

    try {
      const response = await GenerationApiClient.generateStory(request);
      setStory(response.story);
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'An unknown error occurred';
      setError(errorMessage);
      console.error('Story generation failed:', err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="app" role="main">
      <header className="app-header">
        <h1>AI Story Generator</h1>
        <p>Create custom stories based on your preferences</p>
      </header>

      <div className="app-container">
        <aside className="preferences-panel" role="region" aria-label="Story preferences">
          <StoryForm onSubmit={handleGenerateStory} isLoading={isLoading} />
        </aside>

        <main className="story-panel" role="region" aria-label="Generated story">
          <StoryViewer story={story} isLoading={isLoading} error={error} />
        </main>
      </div>
    </div>
  );
}
