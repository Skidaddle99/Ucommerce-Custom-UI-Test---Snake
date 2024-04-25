class SnakeGame extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this.width = 800; // Double the width
    this.height = 400;
    this.scale = 20;
    this.snake = [{ x: 10, y: 10 }];
    this.food = this.generateFood();
    this.direction = 'right';
    this.score = 0;
  }

  connectedCallback() {
    this.setupCanvas();
    this.focus();
    this.addEventListener('keydown', this.handleKeyDown.bind(this));
    this.gameLoop();
  }

  setupCanvas() {
    const container = document.createElement('div');
    container.style.display = 'flex';
    container.style.justifyContent = 'center';
    container.style.alignItems = 'center';
    container.style.height = this.height + 'px'; // Set container height to match the game height
    this.shadowRoot.appendChild(container);

    const canvas = document.createElement('canvas');
    canvas.width = this.width;
    canvas.height = this.height;
    container.appendChild(canvas);
    this.ctx = canvas.getContext('2d');
    this.tabIndex = 0;

    // Prevent page scroll with arrow keys
    this.addEventListener('keydown', function (e) {
      if(['ArrowUp', 'ArrowDown'].includes(e.key)) {
        e.preventDefault();
      }
    });
  }

  render() {
    // Clear the canvas
    this.ctx.clearRect(0, 0, this.width, this.height);

    // Draw outline
    this.ctx.strokeStyle = 'black';
    this.ctx.lineWidth = 2;
    this.ctx.strokeRect(0, 0, this.width, this.height);

    // Draw the snake
    this.ctx.fillStyle = 'green';
    this.snake.forEach(segment => {
      this.ctx.fillRect(segment.x * this.scale, segment.y * this.scale, this.scale, this.scale);
    });

    // Draw the food
    this.ctx.fillStyle = 'red';
    this.ctx.fillRect(this.food.x * this.scale, this.food.y * this.scale, this.scale, this.scale);

    // Display score
    this.ctx.fillStyle = 'black'; // Change color to black
    this.ctx.font = '20px Arial';
    this.ctx.fillText('Score: ' + this.score, 10, 30);
  }

  handleKeyDown(event) {
    switch (event.key) {
      case 'ArrowUp':
        if (this.direction !== 'down') this.direction = 'up';
        break;
      case 'ArrowDown':
        if (this.direction !== 'up') this.direction = 'down';
        break;
      case 'ArrowLeft':
        if (this.direction !== 'right') this.direction = 'left';
        break;
      case 'ArrowRight':
        if (this.direction !== 'left') this.direction = 'right';
        break;
    }
  }

  gameLoop() {
    this.intervalId = setInterval(() => { // Store interval ID
      this.moveSnake();
      this.checkCollision();
      this.render();
    }, 100);
  }

  moveSnake() {
    const head = { ...this.snake[0] };

    switch (this.direction) {
      case 'up':
        head.y -= 1;
        break;
      case 'down':
        head.y += 1;
        break;
      case 'left':
        head.x -= 1;
        break;
      case 'right':
        head.x += 1;
        break;
    }

    this.snake.unshift(head);

    if (head.x === this.food.x && head.y === this.food.y) {
      this.food = this.generateFood();
      this.score += 10;
      this.callEndpoint(); // Call the endpoint when the snake gets an apple
    } else {
      this.snake.pop();
    }
  }

  checkCollision() {
    const head = this.snake[0];
    // Check wall collision
    if (head.x < 0 || head.x >= this.width / this.scale || head.y < 0 || head.y >= this.height / this.scale) {
      this.resetGame();
    }
    // Check self collision
    for (let i = 1; i < this.snake.length; i++) {
      if (head.x === this.snake[i].x && head.y === this.snake[i].y) {
        this.resetGame();
      }
    }
  }

  generateFood() {
    return {
      x: Math.floor(Math.random() * (this.width / this.scale)),
      y: Math.floor(Math.random() * (this.height / this.scale))
    };
  }

  resetGame() {
    this.snake = [{ x: 10, y: 10 }];
    this.food = this.generateFood();
    this.direction = 'right';
    this.score = 0;
    clearInterval(this.intervalId); // Clear the game loop interval
    this.gameLoop(); // Restart the game loop
  }

  callEndpoint() {
    fetch('/product', {
      method: 'POST', // Change the method as needed (GET, POST, etc.)
      headers: {
        'Content-Type': 'application/json'
      }
    })
        .then(response => {
          // Handle the response as needed
          console.log('Endpoint called successfully');
        })
        .catch(error => {
          console.error('Error calling endpoint:', error);
        });
  }
}

customElements.define('snake-game', SnakeGame);
