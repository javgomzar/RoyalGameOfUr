const board = document.getElementById("board");
const turn = document.querySelector(".turn");
const turn_img = turn.querySelector("img");
const black_pieces = document.querySelectorAll(".black");
const white_pieces = document.querySelectorAll(".white");
const dice_button = document.querySelector(".dice");
const dice_result = document.querySelector(".dice-result");

const BLACK = "Black";
const WHITE = "White";

function opposite(color) {
    return color === BLACK ? WHITE : BLACK;
}

const images = {
    Black: "images/Black.png",
    White: "images/White.png",
};

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

let currentTurn = WHITE;
let dice = null;

function nextTurn(color) {
    dice = null;
    dice_result.textContent = "";
    turn_img.src = images[color];
    currentTurn = color;
}

const rosettas = [4, 8, 14];
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

let draggedElement = null;
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
    }
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

function isValidPosition(offsetX, offsetY) {
    if (!isInBoard(offsetX, offsetY)) {
        return false;
    }
    if (currentTurn === WHITE) {
        return offsetX <= 120;
    }
    if (currentTurn === BLACK) {
        return offsetX >= 60;
    }
}

document.addEventListener('mousemove', (event) => {
    if (!draggedElement) return;
    draggedElement.style.left = (event.clientX - 30) + 'px';
    draggedElement.style.top = (event.clientY - 30) + 'px';
});

document.addEventListener('mouseup', (event) => {
    if (event.button === 0) {
        if (!draggedElement) return;

        let left = 0;
        let top = 0;
        const board_rect = board.getBoundingClientRect();
        const offsetX = event.clientX - board_rect.left;
        const offsetY = event.clientY - board_rect.top;
        const isValid = isValidPosition(offsetX, offsetY);
        console.log("Valid: " + isValid);
        if (isValid) {
            ({left, top} = snap(offsetX, offsetY));

            let lastSteps = 0;
            let lastOffsetX = lastX - board_rect.left;
            let lastOffsetY = lastY - board_rect.top;
            if (isInBoard(lastOffsetX, lastOffsetY)) {
                lastSteps = getSteps(lastOffsetX, lastOffsetY);
            }

            let nextSteps = getSteps(left, top);
            console.log(lastSteps, nextSteps)
            if (nextSteps - lastSteps === dice) {
                left += board_rect.left;
                top += board_rect.top;

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
        }
        else {
            left = lastX;
            top = lastY;
        }
        draggedElement.style.left = left + 'px';
        draggedElement.style.top = top + 'px';
        draggedElement.style.zIndex = 1;

        draggedElement = null;
        lastX = null;
        lastY = null;
    }
});

document.body.addEventListener('mouseleave', (event) => {
    if (draggedElement) {
        draggedElement.style.left = lastX + 'px';
        draggedElement.style.top = lastY + 'px';

        draggedElement = null;
        lastX = null;
        lastY = null;
    }
});

board.addEventListener('dragstart', (event) => {
    event.preventDefault();
})

function addPiece(element, color, left, top) {
    element.style.left = left + 'px';
    element.style.top  = top + 'px';

    element.addEventListener('mousedown', (event) => {
        if (currentTurn === color && event.button === 0 && dice !== null) {
            draggedElement = element;
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

for (let i = 0; i < 7; i++) {
    const board_rect = board.getBoundingClientRect();
    const top = board_rect.top + i * 60.0 - 240;

    const white_piece = white_pieces[i];
    addPiece(white_piece, WHITE, board_rect.left - 150, top);

    const black_piece = black_pieces[i];
    addPiece(black_piece, BLACK, board_rect.left + 90, top);
}

async function main() {

}

main();