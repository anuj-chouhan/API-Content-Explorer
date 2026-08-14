# index.html — Desktop Presentation Changes Log

All changes below are scoped to **desktop only** (`@media (min-width: 769px)` or the desktop `else` branch in JS). Mobile is never touched.

---

## 1. Added a `<style>` block inside `<head>`

Right after the existing `<link rel="stylesheet" href="TemplateData/style.css">` line, a full `<style>...</style>` block was added. This is where almost everything below lives.

## 2. Desktop page background

```css
html, body { background: #D8E8EA; }
```
(inside the `@media (min-width: 769px)` block)

## 3. Footer text changed

`#unity-build-title` text changed from `API Content Explorer` to:
```
Click here to enter fullscreen
```
Also given `cursor: pointer;` in CSS so it visually looks clickable.

## 4. Footer text made clickable (same action as fullscreen icon)

In the JS, inside `.then((unityInstance) => { ... })`, added:
```js
const enterFullscreen = () => {
  unityInstance.SetFullscreen(1);
};
document.querySelector("#unity-fullscreen-button").onclick = enterFullscreen;
document.querySelector("#unity-build-title").onclick = enterFullscreen;
```
(Replaces the old single-line `#unity-fullscreen-button` onclick assignment.)

## 5. Unity logo removed from footer

```css
#unity-logo-title-footer { display: none !important; }
```

## 6. Viewport border/frame added

- New empty `<div id="unity-viewport-frame" aria-hidden="true"></div>` added in the HTML, right after `<canvas id="unity-canvas">`.
- CSS gives it the rounded-border look:
```css
#unity-viewport-frame {
  position: absolute;
  display: none;
  box-sizing: border-box;
  border-radius: 18px;
  border: 2px solid rgba(0, 0, 0, 0.12);
  pointer-events: none;
  z-index: 2;
}
#unity-viewport-frame.is-active { display: block; }
```
- In JS (desktop `else` branch), a `positionDesktopOverlays()` function positions it to match the canvas, with your finalized offset values:
```js
viewportFrame.style.left = (canvas.offsetLeft - 8) + "px";
viewportFrame.style.top = canvas.offsetTop + "px";
viewportFrame.style.width = (canvas.offsetWidth + 12) + "px";
viewportFrame.style.height = (canvas.offsetHeight + 6) + "px";
```
- It runs on load and on window `resize`.
- **Important:** `is-active` (which makes it visible) is only added later — inside `.then((unityInstance) => {...})`, at the same moment the loading bar is hidden — so the border doesn't show up during loading.

## 7. Loading screen — Unity logo hidden

```css
#unity-logo { display: none !important; }
```

## 8. Loading bar — resized, recolored, recentered

```css
#unity-progress-bar-empty {
  position: relative;
  background-image: none !important;
  background-color: rgba(0, 0, 0, 0.25) !important;
  width: 420px;
  max-width: 70%;
  height: 12px;
  min-height: 12px;
  border-radius: 999px;
  margin: 0 auto;
  overflow: hidden;
  padding: 0;
  border: none;
}

#unity-progress-bar-full {
  position: absolute !important;
  top: 0 !important;
  left: 0 !important;
  background-image: none !important;
  background-color: #3E9AAE !important;
  height: 100% !important;
  min-height: 12px;
  border-radius: 999px;
  width: 0%;
  margin: 0 !important;
  padding: 0 !important;
  border: none;
  transition: width 0.15s ease-out;
}
```
(`background-image: none` was the key fix — Unity's default bar uses thin PNG sprites, which is why it looked "thin" before.)

In JS, `positionDesktopOverlays()` also recenters `#unity-loading-bar` on the canvas's actual center:
```js
loadingBarEl.style.position = "absolute";
loadingBarEl.style.left = (canvas.offsetLeft + canvas.offsetWidth / 2) + "px";
loadingBarEl.style.top = (canvas.offsetTop + canvas.offsetHeight / 2) + "px";
loadingBarEl.style.transform = "translate(-50%, -50%)";
```

## 9. Fullscreen controls vertical offset (easy-to-edit variable)

```css
:root {
  --fullscreen-controls-offset-y: 16px;
}
/* ... */
#unity-footer {
  transform: translateY(var(--fullscreen-controls-offset-y));
}
```
Change the single `16px` value at the top of the `<style>` block to move the footer controls up/down.

---

# How to replicate these changes in a future Unity export

Every time Unity rebuilds, it regenerates a fresh, "default" `index.html` — your customizations get wiped because Unity has no idea you made them.

You've got two realistic options:

### Option A — Manual copy-paste (fine for occasional rebuilds)
Keep this current customized `index.html` saved somewhere safe (e.g. `index.custom.html`, outside your Unity build output folder so it doesn't get overwritten). When Unity generates a new one:
1. Open both files side by side.
2. Copy your `<style>` block from the old file into the new file's `<head>`.
3. Copy the HTML additions (`#unity-viewport-frame` div, changed footer text) into the new file's body.
4. Copy the JS changes (the `positionDesktopOverlays()` function, the `enterFullscreen` wiring) into the new file's `<script>`.
5. **Leave alone**: `buildUrl`, `loaderUrl`, `dataUrl`, `frameworkUrl`, `codeUrl`, `productVersion` — these come from your new build and must stay as Unity generated them.

This log doc is meant to make step 2–4 fast and mistake-proof — just match section-by-section.

### Option B — Automate it with a small script (better if you rebuild often)
Since Unity's default template structure is consistent between builds, you can write a short script that takes a **freshly generated** `index.html` and automatically re-applies your customizations to it. The only things that differ between builds are the `buildUrl`/`loaderUrl`/`dataUrl`/`frameworkUrl`/`codeUrl`/`productVersion` lines — everything else in your customized version can just be dropped in as-is.

If you want, I can write you a script (Python or Node) that:
1. Reads the new Unity-exported `index.html`.
2. Extracts just the build-specific config values from it (the file paths/version).
3. Takes your saved customized template and swaps only those values in.
4. Outputs a ready-to-use `index.html` with all your customizations already applied.

That way, after every Unity rebuild you'd just run one command instead of manually re-pasting anything. Want me to build that script for you?
