# Blob Divide

<div align="center">

![Blob Divide Banner](https://via.placeholder.com/1200x300/2C3E50/ECF0F1?text=BLOB+DIVIDE)

**A 3D puzzle platformer featuring a Blob with a split mechanic.**

*Split, solve, merge - master the art of being in two places at once*

[![Unity Version](https://img.shields.io/badge/Unity-6.2-black?logo=unity)](https://unity.com/)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Made with Love](https://img.shields.io/badge/Made%20with-❤️-red.svg)](https://github.com/yourusername/blob-divide)


</div>

---

## 🎮 Inspiration

**Blob Divide** draws heavy inspiration from Valve's iconic *Portal 2* test chambers, reimagining the puzzle-solving experience with a unique split-merge mechanic. Instead of creating portals, you split yourself.

> *"What if the companion cube could split in two and solve puzzles independently?"*

**Key Inspirations:**
- **Portal 2** — Test chamber design, puzzle philosophy, environmental storytelling
- **Brothers: A Tale of Two Sons** — Dual-character control scheme

---

## 🧩 Game Logic & Mechanics

### Core Mechanic: Split & Merge

The heart of Blob Divide is the split-merge system:

```
Single Blob (E) → Split → BlobA (Green) + BlobB (Cyan)
                              ↓                ↓
                         (Solve Puzzle)   (Solve Puzzle)
                              ↓                ↓
                         Move Close ← → Merge → Complete Level
```

### Controls

| State | Control | BlobA (Green) | BlobB (Cyan) |
|-------|---------|---------------|---------------|
| **Movement** | 3D Movement | WASD | Arrow Keys |
| **Jump** | Jump | Space | Right Shift |
| **Split** | Split into two | E | — |
| **Merge** | Auto-merge | Proximity (3.0 units) | Proximity (3.0 units) |
| **Camera** | Switch view | Tab | Tab |

### Puzzle Elements

#### 🔵 Pressure Buttons
- Require weight to activate (blob or box)
- Control gates and doors
- Must be held down continuously

#### 🚪 Gates
- Controlled by pressure buttons
- Block passage until activated
- Multiple gates can share buttons

#### 🪨 Pushable Boxes
- Can be pushed by walking into them
- Have weight and physics (mass: 50, gravity enabled)
- Fall off ledges realistically

#### 🚪 Exit Door
- Only opens for merged blob
- Rejects split blobs with red flash
- Glows bright green on successful completion
- Triggers level complete sequence

### Game Rules

1. **Split Timer**: 30 seconds before forced merge
2. **Merge Distance**: Blobs auto-merge within 3.0 units
3. **Completion**: Must be merged to exit level
4. **Death**: Fall off map = restart (planned)
5. **No Combat**: Pure puzzle-solving experience

---

## 🎯 Features

### ✅ Implemented

- [x] **Split/Merge System** — Seamless blob division and recombination
- [x] **Dual Character Control** — Simultaneous WASD + Arrow key control
- [x] **3D Movement** — Full freedom of movement in all directions
- [x] **Dynamic Camera** — Behind-blob perspective with Tab switching
- [x] **Physics-Based Puzzles** — Realistic stone pushing, gravity, collision
- [x] **Animated Character** — Walk and idle animations with smooth blending
- [x] **Visual Feedback** — Color-coded blobs, glowing doors, button states
- [x] **Portal 2-Style Chambers** — Clean, enclosed test chamber aesthetic
- [x] **Pressure Button System** — Multi-button support, gate control
- [x] **Pushable Objects** — Weighted stones with realistic physics

### 🚧 In Progress

- [ ] **Level Progression** — Multiple test chambers with increasing difficulty
- [ ] **Sound Design** — Footsteps, button presses, ambient chamber sounds
- [ ] **UI/HUD** — Timer display, controls hint, level number
- [ ] **Death/Respawn** — Fall detection and checkpoint system

---

## 🏗️ Technical Details

### Built With

- **Engine:** Unity 6.2 (Built-in Render Pipeline)
- **Language:** C#
- **Platform:** PC (Windows/Mac/Linux)
- **Art Style:** Low-poly minimalist
- **Target:** 60 FPS

### Core Scripts

| Script | Purpose |
|--------|---------|
| `GameManager.cs` | Split/merge logic, level flow control |
| `BlobController.cs` | Character movement, input handling, animations |
| `CameraController.cs` | Dynamic camera following, view switching |
| `PushableStone.cs` | Physics-based pushable object behavior |
| `PressureButton.cs` | Button activation, gate control |
| `ExitDoor.cs` | Level completion detection, visual feedback |

---

## 📁 Project Structure

```
BlobDivide/
├── Assets/
│   ├── Scenes/
│   │   └── TestChamber_01.unity
│   ├── Scripts/
│   │   ├── GameManager.cs
│   │   ├── BlobController.cs
│   │   ├── CameraController.cs
│   │   ├── PushableStone.cs
│   │   ├── PressureButton.cs
│   │   └── ExitDoor.cs
│   ├── Characters/
│   │   ├── Models/
│   │   ├── Materials/
│   │   ├── Textures/
│   │   └── Animations/
│   ├── Prefabs/
│   │   ├── Blob.prefab
│   │   ├── Stone.prefab
│   │   ├── PressureButton.prefab
│   │   └── Gate.prefab
│   ├── Materials/
│   │   ├── BlobMaterial.mat
│   │   ├── FloorMaterial.mat
│   │   └── GateMaterial.mat
│   └── Animations/
│       └── CharacterAnimator.controller
├── Documentation/
│   ├── setup-guide.md
│   ├── level-design-guide.md
│   └── troubleshooting.md
└── README.md
```

---

## 🚀 Getting Started

### Prerequisites

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/blob.git
   cd blob
   ```

2. **Open in Unity**
   - Open Unity Hub
   - Click "Add" → Select `blob` folder
   - Open project (Unity will import assets)

3. **Open the test scene**
   - Navigate to `Assets/Scenes/`
   - Open `Main.unity`

4. **Press Play!**
   - Test character movement with WASD
   - Press E to split
   - Solve the puzzle
   - Merge and complete the level

---

## 🎮 How to Play

### Objective
Navigate through test chambers by splitting yourself into two blobs, solving puzzles that require presence in multiple locations, then merging to reach the exit.

### Basic Puzzle Flow

1. **Start**: Spawn as single merged blob
2. **Split** (E): Divide into green and cyan blobs
3. **Control Both**: 
   - Green blob with WASD + Space
   - Cyan blob with Arrows + Right Shift
4. **Solve Puzzle**: Use both blobs simultaneously
   - Hold buttons
   - Push stones
   - Navigate gates
5. **Merge**: Move blobs close together (auto-merge at 3 units)
6. **Exit**: Enter the door as merged blob

### Example Puzzle Solution

**Chamber 01: Two-Button Gate**

```
Problem: Gate blocks path, needs two buttons pressed

Solution:
1. Split blob (E)
2. Position BlobA (green) on Button 1
3. Switch camera to BlobB (Tab)
4. Move BlobB (cyan) through opened gate to Button 2
5. Both buttons held → Gate stays open
6. Move both blobs to exit area
7. Blobs auto-merge (get within 3 units)
8. Enter exit door → Level complete!
```
---

## 📝 Contributing

Contributions are welcome! Whether it's:

- 🐛 Bug reports
- 💡 Feature suggestions
- 🎨 Art/model contributions
- 📚 Documentation improvements
- 🎮 Level designs

### How to Contribute

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📜 License

This project is licensed under the MIT License.

---

## 🙏 Acknowledgments

### Inspiration & References
- **Valve** — Portal 2 for test chamber design inspiration
- **Unity Community** — Tutorials and assets
- **OpenGameArt** — Free assets and character models

---

<div align="center">

**Made with ❤️ and Unity**

[⬆ Back to Top](#blob-divide)

</div>
