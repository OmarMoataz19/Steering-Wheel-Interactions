
### Overview

This project explores the use of gestural interfaces as an alternative interaction technique for controlling car infotainment systems. Instead of relying on traditional touchscreens or physical buttons, the implementation utilizes a combination of Python neural networks (NN) with Keras and a Unity-based driving simulator.

### Video Previews

Check out video previews of the project on [Google Drive](link_to_video_previews).

### Implementation

The project workflow involves the following key steps:

1. **Hand Landmark Extraction:**
    - RGB channels of the input image are extracted.
    - A convolutional neural network is employed to extract hand landmarks.
    - Google's MediaPipe library is used to handle this part of the pipeline, providing a robust solution for hand landmark extraction.

2. **Preprocessing and Neural Network:**
    - Hand landmarks are preprocessed.
    - A simple Feedforward neural network is used to interpret the landmarks.
    - The decision to work with pixel coordinates rather than pixel values simplifies the task, overcoming challenges in the traditional approach.

3. **Camera Placement:**
    - The camera is positioned on the ceiling of the car to capture gestures performed in front or behind the steering wheel.

4. **Sockets:**
   - Detected gestures are communicated from the Python code to the Unity code using sockets.
     
6. **Unity-based Driving Simulator:**
    - A Unity environment is created with arcs above the steering wheel, allowing the driver to visualize and interact with various car systems using gestures.
      
7. **Gesture Mapping:**
    - The Unity code maps the detected gestures to control different infotainment systems.
  
### MediaPipe Hand Landmarks

![MediaPipe Hand Landmarks](link_to_image)

The image above illustrates the 21 landmarks produced by Google's MediaPipe library.

### Experiment Results

An experiment was conducted to compare the user experience and safety between gestural interfaces and touch interfaces. Surprisingly, users with gestural interfaces experienced fewer accidents compared to those using touch interfaces. This unexpected result suggests the potential benefits of gestural interfaces in enhancing safety during driving.

### Usage

1. **Dependencies:**
    - Python 
    - Keras
    - MediaPipe
    - Unity 3D

2. **Setup:**
    - Ensure all dependencies are installed.
    - Run the Unity driving simulator.
    - Execute the Python **app** script for gesture recognition.

3. **Interaction:**
    - Perform gestures in front or behind the steering wheel to control various infotainment systems in the Unity environment.


### Folder Structure

- `Driving Simulator/`: Contains Unity project files.
- `Classifiers/`: Includes Python scripts for hand landmark extraction and gesture recognition.

### Acknowledgments

- This project relies on Google's MediaPipe library for robust hand landmark extraction.
- The Unity environment is built using the Unity 3D engine.
