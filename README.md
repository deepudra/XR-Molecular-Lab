## **Project: Interactive VR Chemistry Lab**

This repository contains a high-fidelity **Virtual Reality Chemistry Simulation** built in **Unity**, designed for the **Meta Quest** (2, 3, and Pro). The project focuses on procedural molecule assembly, real-time feedback systems, and an intuitive XR user interface.

---

## **Key Features**

### **1. Procedural Molecule Assembly**
* **Dynamic Bonding**: Atoms are detected using an `OverlapSphere` and matched against a `MoleculeDatabase`.
* **Visual Bond Generation**: Bonds are drawn procedurally between atoms with custom materials for **Single**, **Double**, **Triple**, and **Covalent** types.
* **DOTween Animations**: Smooth assembly animations using `Ease.OutElastic` and `Ease.OutBack` for atom positioning and scale.

### **2. Intuitive XR Interaction**
* **Near-Far Interactors**: Supports both direct grabbing and ray-casting for object manipulation.
* **Velocity Tracking**: Molecules utilize high linear and angular damping for stable physical handling in VR.
* **Dynamic Highlights**: Custom outline system that highlights the entire molecule structure when hovered or selected.

### **3. HUD & UI Systems**
* **Molecule Info Popup**: A contextual UI triggered by the controller's secondary button that displays the Name, Formula (with rich-text subscripts), and Bond Type of the held item.
* **Discovery Notifications**: A left-hand notification system that alerts the user when a "New Molecule" is discovered, preventing duplicate alerts via a `HashSet`.
* **Atom Spawn Menu**: A smooth pop-up menu on the right hand for spawning various elements (H, O, C, N).

---

## **Technical Stack**
* **Engine**: Unity 2022.3+ (URP) or Unity 6+ (URP).
* **XR Framework**: XR Interaction Toolkit (XRI).
* **Animation**: DOTween (Digital Ruby).
* **UI**: TextMesh Pro with Rich Text support.
* **Target Hardware**: Meta Quest 2, 3, Pro (Android platform).

---

## **Setup & Installation**

### **Prerequisites**
1. **Unity Hub**: Install the latest **LTS version** with **Android Build Support**.
2. **Meta Quest**: Enable **Developer Mode** via the Meta Quest mobile app.

### **Build Instructions**
1. **Switch Platform**: Go to `File > Build Settings`, select **Android**, and set Texture Compression to **ASTC**.
2. **XR Configuration**: In `Project Settings > XR Plug-in Management`, enable the **Oculus** loader for the Android tab.
3. **Optimization**: Ensure Scripting Backend is set to **IL2CPP** and Target Architecture to **ARM64**.
4. **Deploy**: Connect your headset and click **Build and Run**.

---

## **Controls**
* **Grip Button**: Grab atoms or molecules.
* **Primary Button (Right Hand)**: Toggle the Atom Spawner menu.
* **Secondary Button (Left Hand)**: Toggle the detailed Information Popup for the held object.
* **Release (Select Exit)**: Dropping an atom near others triggers the `BondManager` to attempt molecule formation.

---
