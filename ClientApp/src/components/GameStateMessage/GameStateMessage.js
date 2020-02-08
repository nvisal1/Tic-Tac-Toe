import React from 'react';

export const GameStateMessage = ({ winner, isPlayerTurn }) => {
    if (winner === 'inconclusive') {
        if (isPlayerTurn) {
            return (
                <h3>Take your turn</h3>
            );
        }
        return (
            <h3>Your opponent is thinking...</h3>
        )
    }

    else if (winner === 'tie') {
        return (
            <h3>It's a tie!</h3>
        )
    }

    else {
        return (
            <h3>{ winner } wins!</h3>
        );
    }
    
};