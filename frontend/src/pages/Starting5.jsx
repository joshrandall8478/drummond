import { useEffect, useState } from 'react';
function Starting5(){
    const [players, setPlayers] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchPlayers = async () => {
            try {
                setLoading(true);
                const response = await fetch('http://localhost:5206/players');

                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }

                const data = await response.json();
                setPlayers(data);
            } catch (err) {
                console.error('error fetching players:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchPlayers();
    }, []);

    return(
        <>
            <div>
                
            </div>
        </>
    )
}

export default Starting5