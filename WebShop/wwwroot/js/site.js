// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//const circleelement = document.queryselector('.circle');

//// create objects to track mouse position and custom cursor position
//const mouse = { x: 0, y: 0 }; // track current mouse position
//const previousmouse = { x: 0, y: 0 }; // store the previous mouse position
//const circle = { x: 0, y: 0 }; // track the circle position

//// initialize variables to track scaling, rotation, and velocity
//let currentscale = 0; // track current scale value
//let currentangle = 0; // track current angle value
//let smoothedvelocity = 0; // smoothed velocity for jitter reduction

//// weight for smoothing velocity (80% old, 20% new)
//const velocitysmoothingfactor = 0.8;

//// update mouse position on the 'mousemove' event
//window.addeventlistener('mousemove', (e) => {
//    mouse.x = e.x;
//    mouse.y = e.y;
//});

//// smoothing factor for cursor movement speed (0 = smoother, 1 = instant)
//const speed = 0.17;

//// start animation
//const tick = () => {
//    // move
//    // calculate circle movement based on mouse position and smoothing
//    circle.x += (mouse.x - circle.x) * speed;
//    circle.y += (mouse.y - circle.y) * speed;
//    // create a transformation string for cursor translation
//    const translatetransform = `translate(${circle.x}px, ${circle.y}px)`;

//    // squeeze
//    // 1. calculate the change in mouse position (deltamouse)
//    const deltamousex = mouse.x - previousmouse.x;
//    const deltamousey = mouse.y - previousmouse.y;
//    // update previous mouse position for the next frame
//    previousmouse.x = mouse.x;
//    previousmouse.y = mouse.y;
//    // 2. calculate mouse velocity using pythagorean theorem
//    const newvelocity = math.sqrt(deltamousex ** 2 + deltamousey ** 2);
//    // 3. smooth the velocity
//    smoothedvelocity = smoothedvelocity * velocitysmoothingfactor + newvelocity * (1 - velocitysmoothingfactor);
//    // 4. convert smoothed velocity to a value in the range [0, 0.5]
//    const scalevalue = (smoothedvelocity / 150) * 0.5;
//    // 5. smoothly update the current scale
//    currentscale += (scalevalue - currentscale) * speed;
//    // 6. create a transformation string for scaling
//    const scaletransform = `scale(${1 + currentscale}, ${1 - currentscale})`;

//    // rotate
//    // 1. calculate the angle using the smoothed velocity direction
//    if (smoothedvelocity > 1) { // avoid unnecessary updates at low velocity
//        currentangle = math.atan2(deltamousey, deltamousex) * 180 / math.pi;
//    }
//    // 2. create a transformation string for rotation
//    const rotatetransform = `rotate(${currentangle}deg)`;

//    // apply all transformations to the circle element in a specific order: translate -> rotate -> scale
//    circleelement.style.transform = `${translatetransform} ${rotatetransform} ${scaletransform}`;

//    // request the next frame to continue the animation
//    window.requestanimationframe(tick);
//}

//// start the animation loop
//tick();

const circleElement = document.querySelector('.circle');

circleElement.innerHTML = `<div class="text-line">Customer</div>
    <div class="text-line">Help</div>`;

// Create objects to track mouse position and circle position
const mouse = { x: 0, y: 0 }; // Track current mouse position
const circle = { x: 20, y: 100 }; 

// Parameters
const circleSize = 100; // Circle diameter (adjusted for larger size)
const escapeDistance = 150; // Distance threshold for the circle to "run"
const escapeSpeed = 20; // Speed at which the circle "runs away"

// Update mouse position on the 'mousemove' event
window.addEventListener('mousemove', (e) => {
    mouse.x = e.x;
    mouse.y = e.y;
});

// Start animation
const tick = () => {
    // Calculate distance between mouse and circle
    const deltaX = mouse.x - circle.x;
    const deltaY = mouse.y - circle.y;
    const distance = Math.sqrt(deltaX ** 2 + deltaY ** 2); 

    if (distance < escapeDistance) {
        // Calculate the direction to move away from the mouse
        const angle = Math.atan2(deltaY, deltaX);
        circle.x -= Math.cos(angle) * escapeSpeed;
        circle.y -= Math.sin(angle) * escapeSpeed;

        // Keep the circle within the bounds of the viewport
        circle.x = Math.max(circleSize / 2, Math.min(window.innerWidth - circleSize / 2, circle.x));
        circle.y = Math.max(circleSize / 2, Math.min(window.innerHeight - circleSize / 2, circle.y));
    }

    // Apply the transform to move the circle
    const translateTransform = `translate(${circle.x - circleSize / 2}px, ${circle.y - circleSize / 2}px)`;
    circleElement.style.transform = translateTransform;

    // Request the next frame
    window.requestAnimationFrame(tick);
};

// Start the animation loop
tick();

