import { useEffect, useState } from 'react';
import '../css/Starting5.css';

const API_URL = 'http://localhost:5206';

function Starting5() {
    const [criteria, setCriteria] = useState({ category1: '', category2: '' });
    const [score, setScore] = useState(0);
    const [lineup, setLineup] = useState({
        PG: null,
        SG: null,
        SF: null,
        PF: null,
        C: null
    });
    const [selectedPosition, setSelectedPosition] = useState(null);
    const [players, setPlayers] = useState([]);
    const [allPlayers, setAllPlayers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [gameComplete, setGameComplete] = useState(false);
    const [searchQuery, setSearchQuery] = useState('');

    // Initialize game on component mount
    useEffect(() => {
        const initGame = async () => {
            try {
                setLoading(true);

                // fetch all players
                const playersResponse = await fetch('http://localhost:5206/players');
                if (!playersResponse.ok) {
                    throw new Error(`HTTP error! status: ${playersResponse.status}`);
                }
                const playersData = await playersResponse.json();
                setAllPlayers(playersData);

                // start game and get initial criteria
                const gameResponse = await fetch(`${API_URL}/game/start`, {
                    method: 'POST'
                });
                if (!gameResponse.ok) throw new Error('Failed to start game');

                const gameData = await gameResponse.json();
                setCriteria(gameData.criteria);

            } catch (err) {
                console.error('Error initializing game:', err);
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        initGame();
    }, []);

    // Get filled positions
    const getFilledPositions = () => {
        return Object.entries(lineup)
            .filter(([_, player]) => player !== null)
            .map(([position]) => position);
    };

    // select position and show matching players
    const selectPosition = (position) => {
        // if position is already done then just return
        if (lineup[position]) return;

        setSelectedPosition(position);
        setSearchQuery('');

        // Filter players by position
        const availablePlayers = allPlayers.filter(p => p.position === position);
        setPlayers(availablePlayers);
    };

    // filter players based on search query
    const filteredPlayers = players.filter(player => {
        const fullName = `${player.firstName} ${player.lastName}`.toLowerCase();
        return fullName.includes(searchQuery.toLowerCase());
    });

    const selectPlayer = async (player) => {
        if (!selectedPosition) return;

        const isGameComplete = getFilledPositions().length === 4; // 4 because we're about to fill the 5th

        try {
            setLoading(true);
            setError(null);

            const response = await fetch(`${API_URL}/game/select-player`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    playerId: player.playerId,
                    position: selectedPosition,
                    isGameComplete
                })
            });

            if (!response.ok) throw new Error('Failed to select player');

            const data = await response.json();

            // update lineup ui 
            setLineup(prev => ({
                ...prev,
                [selectedPosition]: {
                    ...player,
                    points: data.points
                }
            }));

            setScore(prev => prev + data.points);

            // clear selection
            setSelectedPosition(null);
            setPlayers([]);

            if (isGameComplete) {
                setGameComplete(true);
            } else {
                // update criteria for next round
                setCriteria(data.nextCriteria);
            }
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    // complete screen
    if (gameComplete) {
        return (
            <div className="game-container">
                <div className="complete-screen">
                    <h1 className="game-title">GAME COMPLETE!</h1>
                    <div className="final-score">
                        <h2>Final Score</h2>
                        <div className="score-display">{score}</div>
                    </div>

                    <div className="final-lineup">
                        <h3>Your Starting 5</h3>
                        {Object.entries(lineup).map(([position, player]) => (
                            <div key={position} className="final-lineup-item">
                                <span className="position">{position}</span>
                                <span className="player-name">
                                    {player.firstName} {player.lastName}
                                </span>
                                <span className="points">+{player.points}</span>
                            </div>
                        ))}
                    </div>

                    <button
                        className="play-again-button"
                        onClick={() => window.location.reload()}
                    >
                        Play Again
                    </button>
                </div>
            </div>
        );
    }

    // playing screen
    return (
        <div className="game-container">
            <div className="game-header">
                <h1 className="game-title">STARTING 5</h1>
                <button
                    className="logout-button"
                >
                    LOGOUT
                </button>
            </div>

            {loading && !criteria.category1 ? (
                <div className="loading">Loading game...</div>
            ) : (
                <>
                    <div className="categories-box">
                        <div className="categories-title">CATEGORIES</div>
                        <div className="categories-content">
                            <div className="category">
                                <div className="category-label">Category 1</div>
                                <div className="category-value">{criteria.category1}</div>
                            </div>
                            <div className="category">
                                <div className="category-label">Category 2</div>
                                <div className="category-value-2">{criteria.category2}</div>
                            </div>
                        </div>
                    </div>

                    <div className="score-box">
                        <div className="score-label">CURRENT SCORE</div>
                        <div className="score-value">{score}</div>
                    </div>

                    <div className="lineup-section">
                        {['PG', 'SG', 'SF', 'PF', 'C'].map(position => (
                            <div
                                key={position}
                                className={`position-row ${lineup[position] ? 'filled' : ''} ${selectedPosition === position ? 'selected' : ''}`}
                                onClick={() => !lineup[position] && selectPosition(position)}
                            >
                                <span className="position-label">{position}</span>
                                <span className="position-content">
                                    {lineup[position] ? (
                                        <>
                                            <span className="player-name">
                                                {lineup[position].firstName} {lineup[position].lastName}
                                            </span>
                                            <span className="player-points">+{lineup[position].points}</span>
                                        </>
                                    ) : (
                                        <span className="select-text">Select Player</span>
                                    )}
                                </span>
                            </div>
                        ))}
                    </div>
                </>
            )}

            {selectedPosition && (
                <div className="player-selection-modal">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h2>Select {selectedPosition}</h2>
                            <button
                                className="close-button"
                                onClick={() => {
                                    setSelectedPosition(null);
                                    setPlayers([]);
                                    setSearchQuery('');
                                }}
                            >
                                ✕
                            </button>
                        </div>

                        {/* NEW: Search box */}
                        <div className="search-box">
                            <input
                                type="text"
                                className="search-input"
                                placeholder="Search players..."
                                value={searchQuery}
                                onChange={(e) => setSearchQuery(e.target.value)}
                                autoFocus
                            />
                        </div>

                        <div className="player-list">
                            {loading ? (
                                <div className="loading">Loading...</div>
                            ) : !searchQuery ? (
                                <div className="search-prompt">Start typing to search for players...</div>
                            ) : filteredPlayers.length === 0 ? (
                                <div className="no-players">No players found matching your search</div>
                            ) : (
                                filteredPlayers.map(player => (
                                    <div
                                        key={player.playerId}
                                        className="player-item"
                                        onClick={() => selectPlayer(player)}
                                    >
                                        <span className="player-name">
                                            {player.firstName} {player.lastName}
                                        </span>
                                        <span className="player-position">{player.position}</span>
                                    </div>
                                ))
                            )}
                        </div>
                    </div>
                </div>
            )}

            {error && <div className="error-toast">{error}</div>}
        </div>
    );
}

export default Starting5;