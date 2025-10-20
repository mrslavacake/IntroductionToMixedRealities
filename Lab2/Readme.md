# VR Darts Experience: XR Interaction Toolkit Demo

This repository contains a simple, physics-based VR darts game built in Unity 2022 using the OpenXR and XR Interaction Toolkit (XRIT) packages. The primary goal of this project was to implement core XR mechanics, specifically simulated hand presence, grab/select functionality, physics-based throwing, and a dynamic scoring system that uses the XR Device Simulator for non-VR development.

# Features

Physics-Based Throwing: Darts can be grabbed, thrown, and embedded into the target using realistic physics.

Dynamic Scoring: Score calculation is based on the dart's proximity to the target's center upon impact.

Visual Feedback: Includes particle effects at the point of impact and visual color change on the dartboard when a hit is registered.

Infinite Darts: Implemented a respawn mechanism to ensure an endless supply of darts at the player's starting position.

Cumulative Score Display: Score is tracked cumulatively, showing the total and the score gained from the last successful hit (e.g., "Score: 73 (+48)").

# Video demo is called Homework2 (demo).mp4

# Changed Scoring Logic!!!

The initial project concept included scoring based on the player's distance from the target. However, I deemed this logic irrelevant and nonsensical in the context of a Darts game, where the challenge should focus on accuracy rather than distance from a static spawning point.

Therefore, the score calculation was fundamentally redesigned to rely solely on the dart's proximity to the target center upon impact, mirroring real-world darts scoring.

(The original script logic, which calculated score based on player distance, has been preserved in Assets/Scripts/TargetScoreOld.cs for historical context, but it is not used in the final build.)
