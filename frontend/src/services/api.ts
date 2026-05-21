/**
 * API client for story generation service
 * Handles communication with the backend /api/v1/generate endpoint
 */

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000';

export interface StoryRequest {
  plot: string;
  characters: string;
  setting: string;
  format: 'Prose' | 'Screenplay' | 'Stageplay' | 'Poem';
  length: number;
  genre: string;
  theme: string;
  pointOfView: string;
  dialogueBalance: string;
  additionalContext: string;
}

export interface GeneratedStory {
  storyId: string;
  jobId: string;
  content: string;
  format: string;
  length: number;
  tokensUsed: number;
  safetyFlags: string[];
  createdAt: string;
}

export interface GenerateResponse {
  jobId: string;
  story: GeneratedStory;
}

export class GenerationApiClient {
  /**
   * Generate a story based on user preferences
   */
  static async generateStory(request: StoryRequest): Promise<GenerateResponse> {
    try {
      const response = await fetch(`${API_BASE_URL}/api/v1/generate`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(request),
      });

      if (!response.ok) {
        const error = await response.json().catch(() => ({ message: response.statusText }));
        throw new Error(error.message || `HTTP ${response.status}`);
      }

      const data: GenerateResponse = await response.json();
      return data;
    } catch (error) {
      if (error instanceof Error) {
        throw new Error(`Failed to generate story: ${error.message}`);
      }
      throw error;
    }
  }

  /**
   * Get the status of a generation job
   */
  static async getJobStatus(jobId: string): Promise<any> {
    try {
      const response = await fetch(`${API_BASE_URL}/api/v1/generate/${jobId}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
      });

      if (!response.ok) {
        throw new Error(`HTTP ${response.status}`);
      }

      const data = await response.json();
      return data;
    } catch (error) {
      if (error instanceof Error) {
        throw new Error(`Failed to get job status: ${error.message}`);
      }
      throw error;
    }
  }
}

export default GenerationApiClient;
