import React from 'react'

export default function App() {
  return (
    <div className="app">
      <aside className="panel">
        <h2>Preferences</h2>
        <p>(Form fields will appear here)</p>
      </aside>
      <main className="viewer">
        <h2>Story</h2>
        <textarea readOnly rows={20} cols={60} />
      </main>
    </div>
  )
}
