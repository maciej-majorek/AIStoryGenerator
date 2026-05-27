import React, { useState } from 'react';
import { StoryRequest } from '../services/api';
import '../styles/form.css';

interface StoryFormProps {
  onSubmit: (request: StoryRequest) => void;
  isLoading?: boolean;
}

export const StoryForm: React.FC<StoryFormProps> = ({ onSubmit, isLoading = false }) => {
  const [formData, setFormData] = useState<StoryRequest>({
    plot: '',
    characters: '',
    setting: '',
    format: 'Prose',
    length: 500,
    genre: '',
    theme: '',
    pointOfView: 'Third Person',
    dialogueBalance: 'Balanced',
    additionalContext: '',
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: name === 'length' ? parseInt(value, 10) : value,
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit(formData);
  };

  return (
    <form onSubmit={handleSubmit} className="story-form" role="form" aria-label="Story generation preferences">
      <fieldset disabled={isLoading}>
        <legend>Story Preferences</legend>

        <div className="form-group">
          <label htmlFor="plot">Plot *</label>
          <textarea
            id="plot"
            name="plot"
            value={formData.plot}
            onChange={handleChange}
            placeholder="Describe the main plot of your story..."
            required
            aria-required="true"
            aria-label="Story plot"
          />
        </div>

        <div className="form-group">
          <label htmlFor="characters">Characters *</label>
          <textarea
            id="characters"
            name="characters"
            value={formData.characters}
            onChange={handleChange}
            placeholder="Describe the main characters..."
            required
            aria-required="true"
            aria-label="Story characters"
          />
        </div>

        <div className="form-group">
          <label htmlFor="setting">Setting *</label>
          <textarea
            id="setting"
            name="setting"
            value={formData.setting}
            onChange={handleChange}
            placeholder="Describe the story setting..."
            required
            aria-required="true"
            aria-label="Story setting"
          />
        </div>

        <div className="form-group">
          <label htmlFor="format">Format</label>
          <select
            id="format"
            name="format"
            value={formData.format}
            onChange={handleChange}
            aria-label="Story format"
          >
            <option value="Prose">Prose</option>
            <option value="Screenplay">Screenplay</option>
            <option value="Stageplay">Stageplay</option>
            <option value="Poem">Poem</option>
          </select>
        </div>

        <div className="form-group">
          <label htmlFor="length">
            Length (characters): {formData.length}
          </label>
          <input
            id="length"
            type="range"
            name="length"
            min="50"
            max="5000"
            value={formData.length}
            onChange={handleChange}
            aria-label="Story length in characters"
          />
          <small>50 - 5000 characters</small>
        </div>

        <div className="form-group">
          <label htmlFor="genre">Genre</label>
          <input
            id="genre"
            type="text"
            name="genre"
            value={formData.genre}
            onChange={handleChange}
            placeholder="e.g., Fantasy, Romance, Mystery..."
            aria-label="Story genre"
          />
        </div>

        <div className="form-group">
          <label htmlFor="theme">Theme</label>
          <input
            id="theme"
            type="text"
            name="theme"
            value={formData.theme}
            onChange={handleChange}
            placeholder="e.g., Redemption, Adventure, Loss..."
            aria-label="Story theme"
          />
        </div>

        <div className="form-group">
          <label htmlFor="pointOfView">Point of View</label>
          <select
            id="pointOfView"
            name="pointOfView"
            value={formData.pointOfView}
            onChange={handleChange}
            aria-label="Story point of view"
          >
            <option value="First Person">First Person</option>
            <option value="Second Person">Second Person</option>
            <option value="Third Person">Third Person</option>
            <option value="Third Person Limited">Third Person Limited</option>
            <option value="Third Person Omniscient">Third Person Omniscient</option>
          </select>
        </div>

        <div className="form-group">
          <label htmlFor="dialogueBalance">Dialogue Balance</label>
          <select
            id="dialogueBalance"
            name="dialogueBalance"
            value={formData.dialogueBalance}
            onChange={handleChange}
            aria-label="Story dialogue balance"
          >
            <option value="Minimal">Minimal</option>
            <option value="Light">Light</option>
            <option value="Balanced">Balanced</option>
            <option value="Heavy">Heavy</option>
            <option value="Very Heavy">Very Heavy</option>
          </select>
        </div>

        <div className="form-group">
          <label htmlFor="additionalContext">Additional Context</label>
          <textarea
            id="additionalContext"
            name="additionalContext"
            value={formData.additionalContext}
            onChange={handleChange}
            placeholder="Any other details you'd like the AI to consider..."
            aria-label="Additional story context"
          />
        </div>

        <div className="form-actions">
          <button
            type="submit"
            disabled={isLoading}
            className="btn btn-primary"
            aria-label={isLoading ? 'Generating story...' : 'Generate story'}
          >
            {isLoading ? 'Generating...' : 'Generate Story'}
          </button>
        </div>
      </fieldset>
    </form>
  );
};

export default StoryForm;
