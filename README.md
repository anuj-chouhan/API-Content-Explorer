# API Content Explorer

**Runtime content loading and remote endpoint communication in Unity**

API Content Explorer is a Unity project that demonstrates how an application can request resources from remote endpoints, download them at runtime, convert them into Unity-compatible assets, and present them dynamically, all without bundling that content into the build.

The application supports five content types:

- ✅ **Text**
- ✅ **Images**
- ✅ **Videos**
- ✅ **Audio**
- ✅ **Animated 3D Models**

👉 **[Try It Live](https://api-content-explorer.netlify.app/)**

---

## ✨ Highlights

- 📦 **Multi-Format Runtime Loading**
  Fetches text, images, videos, audio, and 3D models at runtime and converts them into Unity-compatible content.

- 🔁 **Remote Resource Pipeline**
  A complete flow from endpoint request and resource download through runtime preparation, presentation, and playback.

- 📡 **Runtime Status Handling**
  Centralized handling of connection, response, loading, download, and error states, giving users clear feedback at every stage of the loading process.

- 🧭 **Interactive Explorer**
  An Explorer interface where users choose a content type, fetch the remote resource, and preview the result directly at runtime.

- 🎞️ **Animated 3D Models**
  3D models are downloaded, instantiated, and play their animations at runtime.

- 📖 **Built-In Documentation Pages**
  Dedicated **Overview**, **Explorer**, and **Technical** pages explain the project and walk through its architecture and content pipeline.

- 🌐 **Platform-Flexible Architecture**
  The loading system is designed to support Android, iOS, desktop, and WebGL targets. The live demo runs as a WebGL build.

- 🖥️ **Portfolio-Embedded Experience**
  Hosted on Netlify and embedded directly in my portfolio, so visitors can interact with the project instead of only viewing screenshots or videos.

---

## 🚀 Getting Started

### Run the Project in Unity

1. **Clone or download** this repository.
2. Open the project in **Unity 6000.3.22f1** or later.
3. Open the **Demonstration** scene and press **Play** to see the runtime content loading system in action.

### Try the Web Version

No installation needed. Open the **[live demo](https://api-content-explorer.netlify.app/)** in any modern desktop browser.

---

## ⚠️ Important: The `HostedStuffs` Folder

Most folders in this repository are standard parts of the Unity project. The one exception is **`HostedStuffs/`**.

This folder is **not used by Unity directly**. Instead, it holds the remote files the application fetches at runtime:

- Text files
- Images
- Videos
- Audio
- 3D models

Because it lives in a public GitHub repository, each file is reachable through a raw GitHub URL. The application requests these URLs as remote endpoints, downloads the files, and converts them into Unity content while running. In other words, this folder acts as the "server" side of the demo.

> 📝 **Note:** If you move, rename, or delete files in `HostedStuffs/`, the matching URLs in the project will break and that content will fail to load.