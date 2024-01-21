
## Overview

This project explores the use of gestural interfaces as an alternative interaction technique for controlling car infotainment systems. Instead of relying on traditional touchscreens or physical buttons, the implementation utilizes a combination of Python neural networks (NN) with Keras.
This idea seems promising since it reduces the driver's cognitive load and visual demand, hence improving the driver's safety.

## Video Previews

Check out video previews of the project on [Google Drive](https://drive.google.com/drive/folders/1COTy0H4fxzZBZI4VRes5n716vWteKPeu?usp=sharing).

## Implementation

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
### For further implementation details refer to the thesis on [Google Drive](https://drive.google.com/drive/folders/1COTy0H4fxzZBZI4VRes5n716vWteKPeu?usp=sharing).
### The Static gestures model achieved an accuracy of 99.7 percent, while the dynamic gestures model achieved an accuracy of 90 percent. This is Highlighted in the google drive as well.
## MediaPipe Hand Landmarks 

![MediaPipe Hand Landmarks](Landmarks%20image.png)

The image above illustrates the 21 landmarks produced by Google's MediaPipe library.

## Features

### Limited Data Augmentation Requirement

Unlike traditional computer vision projects that heavily rely on extensive data augmentation to achieve robustness, this gestural interface project takes advantage of hand landmarks. The reliance on key landmarks minimizes the need for elaborate data augmentation techniques. The model's focus on the relative positions of these landmarks makes it inherently robust to variations in hand appearance, lighting conditions, and backgrounds.

### High Customizability for User Convenience

One notable feature of this gestural interface system is its exceptional level of customization. Users can tailor the system to their preferences by following a straightforward process:

1. **Capture Gesture Data:**
   - Users are required to take a few photos of themselves performing the desired gestures in front of the camera.

2. **Retraining the Model:**
   - The collected gesture data is used to retrain the neural network. Thanks to the simplified task of interpreting hand landmarks, this retraining process is efficient and requires minimal time.

3. **Personalized System:**
   - After retraining, users can seamlessly use the system with gestures of their choice, making the entire interaction experience highly personalized and convenient.

This customization aspect not only enhances user satisfaction but also makes the system adaptable to individual preferences and requirements.


## Experiment Results

An experiment was conducted to compare the user experience and safety between gestural interfaces and touch interfaces. Surprisingly, users with gestural interfaces experienced fewer accidents compared to those using touch interfaces. This unexpected result suggests the potential benefits of gestural interfaces in enhancing safety during driving. For the experiment results check the Evaluation Section in the thesis on [Google Drive](https://drive.google.com/drive/folders/1COTy0H4fxzZBZI4VRes5n716vWteKPeu?usp=sharing).

## Usage

1. **Dependencies:**
    - Python 
    - Keras
    - Numpy
    - MediaPipe
    - Unity 3D

2. **Setup:**
    - Ensure all dependencies are installed.
    - Run the Unity driving simulator.
    - Execute the Python **app** script for gesture recognition.

3. **Interaction:**
    - Perform gestures in front or behind the steering wheel to control various infotainment systems in the Unity environment.

## Folder Structure

- `Driving Simulator/`: Contains Unity project files.
- `Classifiers/`: Includes Python scripts for hand landmark extraction and gesture recognition.

## Acknowledgments

- This project relies on Google's MediaPipe library for robust hand landmark extraction.
- The Unity environment is built using the Unity 3D engine.
