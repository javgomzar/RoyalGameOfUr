// Board ----------------------------------------------------------------------------
const board = document.getElementById("board");
const rosettas = [4, 8, 14];

board.addEventListener('dragstart', (event) => {
    event.preventDefault();
});

function between(min, value, max) {
    return min <= value && value <= max;
}

function isInBoard(offsetX, offsetY) {
    if (0 <= offsetX && offsetX < 180 && between(0, offsetY, 480)) {
        return between(60, offsetX, 120) || offsetY <= 240 || offsetY >= 360;
    }
    return false;
}

function snap(offsetX, offsetY) {
    let X = Math.floor(offsetX / 60) * 60;
    let Y = Math.floor(offsetY / 60) * 60;

    return {
        left: X,
        top: Y,
    }
}

const black_squares = []
const white_squares = []

let black_x = 120;
let white_x = 0;
let y = 180;
let delta_y = -60;
for (let i = 0; i < 14; i++) {
    black_squares.push([black_x, y])
    white_squares.push([white_x, y]);

    if (i === 3) {
        black_x -= 60;
        white_x += 60;
        delta_y = 60;
    }
    else if (i === 11) {
        black_x += 60;
        white_x -= 60;
        delta_y = -60;
    }
    else {
        y += delta_y;
    }
}

const dice_button = document.querySelector(".dice");
const dice_result = document.querySelector(".dice-result");
let dice = null;

// Pieces ---------------------------------------------------------------------------
const BLACK = "Black";
const WHITE = "White";

const images = {
    Black: "images/Black.png",
    White: "images/White.png",
};

function opposite(color) {
    return color === BLACK ? WHITE : BLACK;
}

const turn = document.querySelector(".turn");
const turn_img = turn.querySelector("img");
let currentTurn = WHITE;

function nextTurn(color) {
    dice = null;
    dice_result.textContent = "";
    turn_img.src = images[color];
    currentTurn = color;
}

class Piece {
    constructor(element, color, left, top) {
        this.element = element;
        this.color = color;
        this.finished = false;
        this.steps = 0;

        element.style.left = left + 'px';
        element.style.top  = top + 'px';

        element.addEventListener('mousedown', (event) => {
            if (
                currentTurn === color && 
                event.button === 0 && 
                dice !== null &&
                !this.finished
            ) {
                draggedPiece = this;
                const rect = element.getBoundingClientRect();
                lastX = rect.left;
                lastY = rect.top;
                element.style.zIndex = 10;
                element.style.cursor = "grabbing";
            }
            event.preventDefault();
        });

        element.addEventListener('mouseup', (event) => {
            if (currentTurn === color && event.button === 0) {
                element.style.cursor = "grab";
            }
            event.preventDefault();
        });
    }
}

const black_piece_elements = document.querySelectorAll(".black");
const white_piece_elements = document.querySelectorAll(".white");

const black_pieces = [];
const white_pieces = [];

const black_free_spots = [];
const white_free_spots = [];

window.addEventListener('load', () => {
    const board_rect = board.getBoundingClientRect();
    for (let i = 0; i < 7; i++) {
        const top = board_rect.top + i * 60.0;

        black_pieces.push(
            new Piece(
                black_piece_elements[i], 
                BLACK, 
                board_rect.left + 180,
                top
            )
        );

        white_pieces.push(
            new Piece(
                white_piece_elements[i], 
                WHITE, 
                board_rect.left - 60,
                top
            )
        );
    }
});

function getSteps(offsetX, offsetY) {
    const exterior_column = currentTurn === WHITE ? 0 : 120;
    if (offsetX === exterior_column) {
        if (offsetY <= 180) {
            return 4 - Math.floor(offsetY / 60);
        }
        else {
            return 14 - Math.floor((offsetY - 360) / 60);
        }
    }
    else {
        return 5 + Math.floor(offsetY / 60);
    }
}

let draggedPiece = null;
let lastX = null;
let lastY = null;

dice_button.addEventListener('mousedown', (event) => {
    if (dice === null) {
        dice = 0;
        for (let i = 0; i < 4; i++) {
            let r = Math.random();
            if (r >= 0.5) {
                dice += 1;
            }
        }
        console.log("Dice roll: " + dice.toString());
        dice_result.textContent = dice.toString();

        if (dice === 0) {
            setTimeout(() => {
                nextTurn(opposite(currentTurn));
            }, 1000);
        }
    }
});

function isValidPosition(offsetX, offsetY) {
    if (isInBoard(offsetX, offsetY)) {
        if (currentTurn === WHITE) {
            return offsetX <= 120;
        }
        if (currentTurn === BLACK) {
            return offsetX >= 60;
        }
    }
    else {
        if (currentTurn === WHITE) {
            return between(0, offsetX, 60) && between(300, offsetY, 360);
        }
        if (currentTurn === BLACK) {
            return between(120, offsetX, 180) && between(300, offsetY, 360);
        }
    }
}

document.addEventListener('mousemove', (event) => {
    if (!draggedPiece) return;
    draggedPiece.element.style.left = (event.clientX - 30) + 'px';
    draggedPiece.element.style.top = (event.clientY - 30) + 'px';
});

document.addEventListener('mouseup', (event) => {
    if (event.button === 0) {
        if (!draggedPiece) return;

        const board_rect = board.getBoundingClientRect();
        const offsetX = event.clientX - board_rect.left;
        const offsetY = event.clientY - board_rect.top;
        let {left, top} = snap(offsetX, offsetY);

        // Check move validity
        let isValid = false;
        let deadPiece = null;
        let nextSteps = 0;
        let lastSteps = 0;

        if (isValidPosition(left, top)) {
            const lastOffsetX = lastX - board_rect.left;
            const lastOffsetY = lastY - board_rect.top;

            if (isInBoard(lastOffsetX, lastOffsetY)) {
                lastSteps = getSteps(lastOffsetX, lastOffsetY);
            }

            nextSteps = getSteps(left, top);
            if (nextSteps - lastSteps === dice) {
                isValid = true;
                const same_color = currentTurn === BLACK ? black_pieces : white_pieces;
                const other_color = currentTurn === BLACK ? white_pieces : black_pieces;
                console.log("Destination: ", left, top);
                for (let i = 0; i < 7; i++) {
                    // Checking same color pieces
                    const same_color_piece = same_color[i];
                    const same_left = same_color_piece.element.offsetLeft - board_rect.left;
                    const same_top = same_color_piece.element.offsetTop - board_rect.top;
                    const same_coordinates = snap(same_left, same_top);
                    if (same_coordinates.left === left && same_coordinates.top === top) {
                        isValid = false;
                        console.log("Same color piece present: invalid move.")
                        break;
                    }
                    
                    // Checking enemy pieces
                    const other_color_piece = other_color[i];
                    const other_left = other_color_piece.element.offsetLeft - board_rect.left;
                    const other_top = other_color_piece.element.offsetTop - board_rect.top;
                    const other_coordinates = snap(other_left, other_top);
                    if (other_coordinates.left === left && other_coordinates.top === top) {
                        deadPiece = other_color_piece;
                        isValid = true;
                        console.log("A piece was taken.")
                        break;
                    }
                }
            }
        }

        console.log("Valid: " + isValid);
        console.log(deadPiece);

        if (isValid) {
            left += board_rect.left;
            top += board_rect.top;

            let free_spots = currentTurn === BLACK ? white_free_spots : black_free_spots;
            if (lastSteps === 0) {
                free_spots.push([lastX, lastY]);
            }

            if (deadPiece !== null) {
                const [free_left, free_top] = free_spots.shift();
                deadPiece.element.style.left = free_left + 'px';
                deadPiece.element.style.top = free_top + 'px';
            }

            if (rosettas.includes(nextSteps)) {
                nextTurn(currentTurn); 
            }
            else {
                nextTurn(opposite(currentTurn));
            }
        }
        else {
            left = lastX;
            top = lastY;
        }

        draggedPiece.element.style.left = left + 'px';
        draggedPiece.element.style.top = top + 'px';
        draggedPiece.element.style.zIndex = 1;

        draggedPiece = null;
        lastX = null;
        lastY = null;
    }
});

document.body.addEventListener('mouseleave', (event) => {
    if (draggedPiece) {
        draggedPiece.element.style.left = lastX + 'px';
        draggedPiece.element.style.top = lastY + 'px';

        draggedPiece = null;
        lastX = null;
        lastY = null;
    }
});
