[1mdiff --cc BlazorApp2/Pages/Team/TeamDisasterDetail.razor.css[m
[1mindex 00f2b8f,f279348..0000000[m
[1m--- a/BlazorApp2/Pages/Team/TeamDisasterDetail.razor.css[m
[1m+++ b/BlazorApp2/Pages/Team/TeamDisasterDetail.razor.css[m
[36m@@@ -1,9 -1,6 +1,15 @@@[m
[32m++<<<<<<< HEAD[m
[32m +﻿/* ═══════════════════════════════════════════════════════════════[m
[32m +   TeamDisasterDetail.razor.css — scoped[m
[32m +   Redesign pass. Same class names as before (no .razor changes[m
[32m +   required) — depth, clearer status language, and dark-themed[m
[32m +   modals that match the rest of the app instead of the generic[m
[32m +   white modal in app.css.[m
[32m++=======[m
[32m+ /* ═══════════════════════════════════════════════════════════════[m
[32m+    TeamDisasterDetail.razor.css — Enhanced UI/UX[m
[32m+    Improved visual hierarchy, better spacing, modern design[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
     ═══════════════════════════════════════════════════════════════ */[m
  [m
  .tdd-page {[m
[36m@@@ -21,13 -19,13 +28,22 @@@[m
      justify-content: space-between;[m
      flex-wrap: wrap;[m
      gap: var(--sp-4);[m
[32m++<<<<<<< HEAD[m
[32m +    padding-bottom: var(--sp-5);[m
[32m +    border-bottom: 1px solid var(--border-color);[m
[32m++=======[m
[32m+     padding-bottom: var(--sp-4);[m
[32m+     border-bottom: 2px solid var(--border-color);[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
  }[m
  [m
  .tdd-title {[m
      font-family: var(--font-display);[m
[32m++<<<<<<< HEAD[m
[32m +    font-size: 28px;[m
[32m++=======[m
[32m+     font-size: 32px;[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
      font-weight: 800;[m
      letter-spacing: -.01em;[m
      color: var(--text-primary);[m
[36m@@@ -41,202 -39,194 +57,369 @@@[m
          display: inline-flex;[m
          align-items: center;[m
          justify-content: center;[m
[32m++<<<<<<< HEAD[m
[32m +        width: 40px;[m
[32m +        height: 40px;[m
[32m +        border-radius: var(--r-md);[m
[32m +        font-size: 17px;[m
[32m +        color: var(--color-critical);[m
[32m +        background: linear-gradient(155deg, var(--color-critical-bg), transparent);[m
[32m +        border: 1px solid rgba(255, 77, 79, .25);[m
[32m++=======[m
[32m+         width: 48px;[m
[32m+         height: 48px;[m
[32m+         border-radius: var(--r-lg);[m
[32m+         font-size: 24px;[m
[32m+         color: #fff;[m
[32m+         background: linear-gradient(135deg, var(--color-critical), #ff6b6b);[m
[32m+         box-shadow: 0 4px 12px rgba(255, 77, 79, 0.3);[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
      }[m
  [m
  /* ── Status badge ────────────────────────────────────────────── */[m
  .tdd-status-badge {[m
      display: inline-flex;[m
      align-items: center;[m
[32m++<<<<<<< HEAD[m
[32m +    gap: 7px;[m
[32m +    padding: 5px 14px;[m
[32m++=======[m
[32m+     gap: 8px;[m
[32m+     padding: 8px 16px;[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
      border-radius: var(--r-full);[m
[31m -    font-size: 13px;[m
[32m +    font-size: 12px;[m
      font-weight: 700;[m
      text-transform: uppercase;[m
      letter-spacing: .05em;[m
      background: var(--bg-surface-2);[m
      color: var(--text-secondary);[m
      border: 1px solid var(--border-color);[m
[32m++<<<<<<< HEAD[m
[32m++=======[m
[32m+     box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
  }[m
  [m
      .tdd-status-badge::before {[m
          content: "";[m
[32m++<<<<<<< HEAD[m
[32m +        width: 7px;[m
[32m +        height: 7px;[m
[32m +        border-radius: 50%;[m
[32m +        background: currentColor;[m
[32m +        flex-shrink: 0;[m
[32m +    }[m
[32m +[m
[32m +/* "InProgress" is the real backend value for an active disaster —[m
[32m +   kept the --active class name so nothing else needs to change,[m
[32m +   just made sure the selector actually matches what's rendered. */[m
[32m +.tdd-status--inprogress,[m
[32m +.tdd-status--active {[m
[32m +    background: var(--color-info-bg);[m
[32m +    color: var(--color-info);[m
[32m +    border-color: rgba(24, 144, 255, .3);[m
[32m++=======[m
[32m+         width: 8px;[m
[32m+         height: 8px;[m
[32m+         border-radius: 50%;[m
[32m+         background: currentColor;[m
[32m+         flex-shrink: 0;[m
[32m+         animation: pulse 2s ease-in-out infinite;[m
[32m+     }[m
[32m+ [m
[32m+ .tdd-status--inprogress,[m
[32m+ .tdd-status--active {[m
[32m+     background: linear-gradient(135deg, rgba(24, 144, 255, 0.1), rgba(24, 144, 255, 0.05));[m
[32m+     color: var(--color-info);[m
[32m+     border-color: rgba(24, 144, 255, 0.3);[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
  }[m
  [m
  .tdd-status--resolved {[m
[31m-     background: var(--color-warning-bg);[m
[32m+     background: linear-gradient(135deg, rgba(250, 173, 20, 0.1), rgba(250, 173, 20, 0.05));[m
      color: var(--color-warning);[m
[32m++<<<<<<< HEAD[m
[32m +    border-color: rgba(250, 173, 20, .3);[m
[32m +}[m
[32m +[m
[32m +.tdd-status--closed {[m
[32m +    background: var(--bg-surface-3);[m
[32m +    color: var(--text-muted);[m
[32m +    border-color: var(--border-color-strong);[m
[32m++=======[m
[32m+     border-color: rgba(250, 173, 20, 0.3);[m
[32m+ }[m
[32m+ [m
[32m+ .tdd-status--closed {[m
[32m+     background: linear-gradient(135deg, rgba(82, 196, 26, 0.1), rgba(82, 196, 26, 0.05));[m
[32m+     color: var(--color-success);[m
[32m+     border-color: rgba(82, 196, 26, 0.3);[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
  }[m
  [m
  .tdd-status--archived {[m
[31m-     background: color-mix(in srgb, var(--text-muted) 12%, transparent);[m
[32m+     background: var(--bg-surface-3);[m
      color: var(--text-muted);[m
[31m-     border: 1px solid color-mix(in srgb, var(--text-muted) 25%, transparent);[m
[32m+     border-color: var(--border-color-strong);[m
[32m+ }[m
[32m+ [m
[32m+     .tdd-status--archived::before {[m
[32m+         display: none;[m
[32m+     }[m
[32m+ [m
[32m+ @keyframes pulse {[m
[32m+     0%, 100% { opacity: 1; }[m
[32m+     50% { opacity: 0.5; }[m
[32m+ }[m
[32m+ [m
[32m+ /* ── Lifecycle Stepper ───────────────────────────────────────── */[m
[32m+ .tdd-stepper {[m
[32m+     display: flex;[m
[32m+     align-items: flex-start;[m
[32m+     gap: var(--sp-3);[m
[32m+     background: linear-gradient(135deg, var(--bg-surface), var(--bg-surface-2));[m
[32m+     border: 1px solid var(--border-color);[m
[32m+     border-radius: var(--r-lg);[m
[32m+     padding: var(--sp-6);[m
[32m+     box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);[m
[32m+ }[m
[32m+ [m
[32m+ .tdd-step {[m
[32m+     display: flex;[m
[32m+     align-items: flex-start;[m
[32m+     gap: var(--sp-3);[m
[32m+     flex: 1;[m
[32m+     min-width: 0;[m
[32m+     position: relative;[m
[32m+ }[m
[32m+ [m
[32m+ .tdd-step-marker {[m
[32m+     flex-shrink: 0;[m
[32m+     width: 40px;[m
[32m+     height: 40px;[m
[32m+     border-radius: 50%;[m
[32m+     display: flex;[m
[32m+     align-items: center;[m
[32m+     justify-content: center;[m
[32m+     font-size: 16px;[m
[32m+     font-weight: 700;[m
[32m+     background: var(--bg-surface-2);[m
[32m+     color: var(--text-muted);[m
[32m+     border: 2px solid var(--border-color-strong);[m
[32m+     transition: all var(--t-normal);[m
[32m+ }[m
[32m+ [m
[32m+ .tdd-step-text {[m
[32m+     display: flex;[m
[32m+     flex-direction: column;[m
[32m+     gap: 4px;[m
[32m+     padding-top: 6px;[m
[32m+     min-width: 0;[m
[32m+     flex: 1;[m
[32m+ }[m
[32m+ [m
[32m+ .tdd-step-label {[m
[32m+     font-family: var(--font-display);[m
[32m+     font-size: 14px;[m
[32m+     font-weight: 700;[m
[32m+     color: var(--text-muted);[m
[32m+     white-space: nowrap;[m
[32m+     transition: color var(--t-normal);[m
[32m+ }[m
[32m+ [m
[32m+ .tdd-step-caption {[m
[32m+     font-size: 12px;[m
[32m+     color: var(--text-secondary);[m
[32m+     line-height: 1.4;[m
[32m+ }[m
[32m+ [m
[32m+ /* ── Step states ─────────────────────────────────────────────– */[m
[32m+ .tdd-step--done .tdd-step-marker {[m
[32m+     background: var(--color-success-bg);[m
[32m+     color: var(--color-success);[m
[32m+     border-color: rgba(82, 196, 26, 0.5);[m
[32m+ }[m
[32m+ [m
[32m+     .tdd-step--done .tdd-step-marker i::before {[m
[32m+         content: "✓";[m
[32m+         font-family: Arial, sans-serif;[m
[32m+     }[m
[32m+ [m
[32m+ .tdd-step--done .tdd-step-label {[m
[32m+     color: var(--text-secondary);[m
[32m+ }[m
[32m+ [m
[32m+ .tdd-step--current .tdd-step-marker {[m
[32m+     background: var(--accent-primary);[m
[32m+     color: #fff;[m
[32m+     border-color: var(--accent-primary);[m
[32m+     box-shadow: 0 0 0 4px var(--accent-glow), 0 4px 12px rgba(59, 130, 246, 0.3);[m
[32m+     transform: scale(1.05);[m
[32m+ }[m
[32m+ [m
[32m+ .tdd-step--current .tdd-step-label {[m
[32m+     color: var(--text-primary);[m
[32m+     font-weight: 800;[m
[32m+ }[m
[32m+ [m
[32m+ .tdd-step--pending .tdd-step-marker {[m
[32m+     opacity: 0.6;[m
[32m+ }[m
[32m+ [m
[32m+ @media (max-width: 768px) {[m
[32m+     .tdd-stepper {[m
[32m+         flex-wrap: wrap;[m
[32m+         gap: var(--sp-2);[m
[32m+         padding: var(--sp-4);[m
[32m+     }[m
[32m+ [m
[32m+     .tdd-step {[m
[32m+         flex: 0 1 calc(50% - var(--sp-1));[m
[32m+     }[m
  }[m
  [m
[32m +    .tdd-status--archived::before {[m
[32m +        display: none;[m
[32m +    }[m
[32m +[m
[32m +/* ── Lifecycle stepper ───────────────────────────────────────── */[m
[32m +.tdd-stepper {[m
[32m +    display: flex;[m
[32m +    align-items: flex-start;[m
[32m +    background: var(--bg-surface);[m
[32m +    border: 1px solid var(--border-color);[m
[32m +    border-radius: var(--r-lg);[m
[32m +    padding: var(--sp-5) var(--sp-6);[m
[32m +    box-shadow: var(--shadow-sm);[m
[32m +}[m
[32m +[m
[32m +.tdd-step {[m
[32m +    display: flex;[m
[32m +    align-items: flex-start;[m
[32m +    gap: var(--sp-3);[m
[32m +    min-width: 0;[m
[32m +}[m
[32m +[m
[32m +.tdd-step-marker {[m
[32m +    flex-shrink: 0;[m
[32m +    width: 34px;[m
[32m +    height: 34px;[m
[32m +    border-radius: 50%;[m
[32m +    display: flex;[m
[32m +    align-items: center;[m
[32m +    justify-content: center;[m
[32m +    font-size: 13px;[m
[32m +    background: var(--bg-surface-2);[m
[32m +    color: var(--text-muted);[m
[32m +    border: 1px solid var(--border-color-strong);[m
[32m +    transition: background var(--t-normal), color var(--t-normal), border-color var(--t-normal), box-shadow var(--t-normal);[m
[32m +}[m
[32m +[m
[32m +.tdd-step-text {[m
[32m +    display: flex;[m
[32m +    flex-direction: column;[m
[32m +    gap: 2px;[m
[32m +    padding-top: 6px;[m
[32m +    min-width: 0;[m
[32m +}[m
[32m +[m
[32m +.tdd-step-label {[m
[32m +    font-family: var(--font-display);[m
[32m +    font-size: 13px;[m
[32m +    font-weight: 700;[m
[32m +    color: var(--text-muted);[m
[32m +    white-space: nowrap;[m
[32m +    transition: color var(--t-normal);[m
[32m +}[m
[32m +[m
[32m +.tdd-step-caption {[m
[32m +    font-size: 12px;[m
[32m +    color: var(--text-secondary);[m
[32m +    line-height: 1.4;[m
[32m +    max-width: 220px;[m
[32m +}[m
[32m +[m
[32m +.tdd-step-connector {[m
[32m +    flex: 1;[m
[32m +    height: 2px;[m
[32m +    margin: 17px var(--sp-2) 0;[m
[32m +    min-width: 24px;[m
[32m +    background: var(--border-color-strong);[m
[32m +    border-radius: var(--r-full);[m
[32m +    transition: background var(--t-normal);[m
[32m +}[m
[32m +[m
[32m +.tdd-step-connector--done {[m
[32m +    background: var(--color-success);[m
[32m +}[m
[32m +[m
[32m +/* Done: filled check, quiet color — it's history, not the focus */[m
[32m +.tdd-step--done .tdd-step-marker {[m
[32m +    background: var(--color-success-bg);[m
[32m +    color: var(--color-success);[m
[32m +    border-color: rgba(82, 196, 26, .35);[m
[32m +}[m
[32m +[m
[32m +.tdd-step--done .tdd-step-label {[m
[32m +    color: var(--text-secondary);[m
[32m +}[m
[32m +[m
[32m +/* Current: the one thing that should draw the eye */[m
[32m +.tdd-step--current .tdd-step-marker {[m
[32m +    background: var(--accent-primary);[m
[32m +    color: #fff;[m
[32m +    border-color: var(--accent-primary);[m
[32m +    box-shadow: 0 0 0 4px var(--accent-glow);[m
[32m +}[m
[32m +[m
[32m +.tdd-step--current .tdd-step-label {[m
[32m +    color: var(--text-primary);[m
[32m +}[m
[32m +[m
[32m +/* Archived reached as "current" is a terminal state, not a warning —[m
[32m +   swap the accent glow for the same muted language as the closed badge. */[m
[32m +.tdd-step--current.tdd-step--archived-final .tdd-step-marker {[m
[32m +    background: var(--bg-surface-3);[m
[32m +    color: var(--text-secondary);[m
[32m +    border-color: var(--border-color-strong);[m
[32m +    box-shadow: none;[m
[32m +}[m
[32m +[m
[32m +@media (max-width: 640px) {[m
[32m +    .tdd-stepper {[m
[32m +        flex-direction: column;[m
[32m +        gap: var(--sp-4);[m
[32m +        padding: var(--sp-4) var(--sp-5);[m
[32m +    }[m
[32m +[m
[32m +    .tdd-step-connector {[m
[32m +        display: none;[m
[32m +    }[m
[32m +[m
[32m +    .tdd-step-caption {[m
[32m +        max-width: none;[m
[32m +    }[m
[32m +}[m
[32m +[m
  /* ── Toast ───────────────────────────────────────────────────── */[m
  .tdd-toast {[m
      display: flex;[m
      align-items: center;[m
      gap: var(--sp-3);[m
[31m-     padding: var(--sp-3) var(--sp-5);[m
[32m+     padding: var(--sp-4) var(--sp-5);[m
      border-radius: var(--r-md);[m
      font-size: 14px;[m
[32m++<<<<<<< HEAD[m
[32m +    font-weight: 500;[m
[32m +    box-shadow: var(--shadow-md);[m
[32m +    animation: toastSlideIn 200ms ease-out;[m
[32m++=======[m
[32m+     font-weight: 600;[m
[32m+     box-shadow: 0 4px 16px rgba(0, 0, 0, 0.15);[m
[32m+     animation: toastSlideIn 300ms cubic-bezier(0.23, 1, 0.320, 1);[m
[32m+     border-left: 4px solid;[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
  }[m
  [m
  .tdd-toast--success {[m
[36m@@@ -269,41 -258,41 +451,66 @@@[m
      align-items: center;[m
      justify-content: center;[m
      gap: var(--sp-3);[m
[31m-     padding: var(--sp-10);[m
[31m-     font-size: 14px;[m
[32m+     padding: var(--sp-12);[m
[32m+     font-size: 15px;[m
      color: var(--text-muted);[m
[32m++<<<<<<< HEAD[m
[32m +    background: var(--bg-surface);[m
[32m +    border: 1px dashed var(--border-color-strong);[m
[32m++=======[m
[32m+     background: linear-gradient(135deg, var(--bg-surface), var(--bg-surface-2));[m
[32m+     border: 2px dashed var(--border-color-strong);[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
      border-radius: var(--r-lg);[m
  }[m
  [m
  .tdd-error {[m
      color: var(--color-critical);[m
[32m++<<<<<<< HEAD[m
[32m +    border-color: rgba(255, 77, 79, .3);[m
[32m++=======[m
[32m+     border-color: rgba(255, 77, 79, 0.3);[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
  }[m
  [m
  .tdd-empty {[m
      display: flex;[m
      flex-direction: column;[m
      align-items: center;[m
[31m-     gap: var(--sp-3);[m
[31m-     padding: var(--sp-12);[m
[32m+     gap: var(--sp-4);[m
[32m+     padding: var(--sp-16);[m
      text-align: center;[m
[32m++<<<<<<< HEAD[m
[32m +    background: var(--bg-surface);[m
[32m +    border: 1px dashed var(--border-color-strong);[m
[32m++=======[m
[32m+     background: linear-gradient(135deg, var(--bg-surface), var(--bg-surface-2));[m
[32m+     border: 2px dashed var(--border-color-strong);[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
      border-radius: var(--r-lg);[m
  }[m
  [m
  .tdd-empty-icon {[m
[32m++<<<<<<< HEAD[m
[32m +    width: 64px;[m
[32m +    height: 64px;[m
[32m++=======[m
[32m+     width: 80px;[m
[32m+     height: 80px;[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
      display: flex;[m
      align-items: center;[m
      justify-content: center;[m
      border-radius: 50%;[m
[32m++<<<<<<< HEAD[m
[32m +    font-size: 26px;[m
[32m +    color: var(--text-muted);[m
[32m +    background: var(--bg-surface-2);[m
[32m++=======[m
[32m+     font-size: 40px;[m
[32m+     color: var(--accent-primary);[m
[32m+     background: var(--accent-glow);[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
  }[m
  [m
  .tdd-empty-title {[m
[36m@@@ -320,34 -309,35 +527,60 @@@[m
  [m
  /* ── Info card ───────────────────────────────────────────────── */[m
  .tdd-info-card {[m
[32m++<<<<<<< HEAD[m
[32m +    position: relative;[m
[32m +    background: var(--bg-surface);[m
[32m +    border: 1px solid var(--border-color);[m
[32m +    border-radius: var(--r-lg);[m
[32m +    padding: var(--sp-6);[m
[32m +    box-shadow: var(--shadow-sm);[m
[32m++=======[m
[32m+     background: linear-gradient(135deg, var(--bg-surface), var(--bg-surface-2));[m
[32m+     border: 1px solid var(--border-color);[m
[32m+     border-radius: var(--r-lg);[m
[32m+     padding: var(--sp-6);[m
[32m+     box-shadow: 0 2px 12px rgba(0, 0, 0, 0.08);[m
[32m+     position: relative;[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
      overflow: hidden;[m
  }[m
  [m
      .tdd-info-card::before {[m
          content: "";[m
          position: absolute;[m
[32m++<<<<<<< HEAD[m
[32m +        inset: 0 0 auto 0;[m
[32m +        height: 2px;[m
[32m +        background: linear-gradient(90deg, var(--accent-primary), var(--color-critical) 130%);[m
[32m +        opacity: .8;[m
[32m++=======[m
[32m+         top: 0;[m
[32m+         left: 0;[m
[32m+         right: 0;[m
[32m+         height: 3px;[m
[32m+         background: linear-gradient(90deg, var(--accent-primary), var(--color-critical));[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
      }[m
  [m
  .tdd-info-grid {[m
      display: grid;[m
[32m++<<<<<<< HEAD[m
[32m +    grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));[m
[32m +    gap: var(--sp-5) var(--sp-6);[m
[32m++=======[m
[32m+     grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));[m
[32m+     gap: var(--sp-6);[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
  }[m
  [m
  .tdd-info-item {[m
      display: flex;[m
      flex-direction: column;[m
[32m++<<<<<<< HEAD[m
[32m +    gap: 5px;[m
[32m++=======[m
[32m+     gap: 6px;[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
  }[m
  [m
  .tdd-info-item--full {[m
[36m@@@ -358,15 -348,15 +591,26 @@@[m
      font-size: 11px;[m
      font-weight: 700;[m
      text-transform: uppercase;[m
[32m++<<<<<<< HEAD[m
[32m +    letter-spacing: .07em;[m
[32m++=======[m
[32m+     letter-spacing: .08em;[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
      color: var(--text-muted);[m
  }[m
  [m
  .tdd-info-value {[m
[32m++<<<<<<< HEAD[m
[32m +    font-size: 14.5px;[m
[32m +    font-weight: 500;[m
[32m +    color: var(--text-primary);[m
[32m +    line-height: 1.5;[m
[32m++=======[m
[32m+     font-size: 15px;[m
[32m+     font-weight: 600;[m
[32m+     color: var(--text-primary);[m
[32m+     line-height: 1.6;[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
  }[m
  [m
  /* ── Actions bar ─────────────────────────────────────────────── */[m
[36m@@@ -375,11 -365,11 +619,15 @@@[m
      flex-wrap: wrap;[m
      align-items: center;[m
      gap: var(--sp-3);[m
[31m-     padding: var(--sp-4) var(--sp-5);[m
[31m-     background: var(--bg-surface);[m
[32m+     padding: var(--sp-5) var(--sp-6);[m
[32m+     background: linear-gradient(135deg, var(--bg-surface), var(--bg-surface-2));[m
      border: 1px solid var(--border-color);[m
      border-radius: var(--r-lg);[m
[32m++<<<<<<< HEAD[m
[32m +    box-shadow: var(--shadow-sm);[m
[32m++=======[m
[32m+     box-shadow: 0 2px 12px rgba(0, 0, 0, 0.08);[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
  }[m
  [m
  .tdd-action-btn {[m
[36m@@@ -393,13 -383,15 +641,20 @@@[m
      font-family: var(--font-body);[m
      cursor: pointer;[m
      border: none;[m
[31m -    transition: all var(--t-fast);[m
[32m +    transition: transform var(--t-fast), box-shadow var(--t-fast), background var(--t-fast), opacity var(--t-fast);[m
      text-decoration: none;[m
[32m+     letter-spacing: 0.02em;[m
[32m+     box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);[m
  }[m
  [m
      .tdd-action-btn:hover:not(:disabled) {[m
[32m++<<<<<<< HEAD[m
[32m +        transform: translateY(-1px);[m
[32m +        box-shadow: var(--shadow-sm);[m
[32m++=======[m
[32m+         transform: translateY(-2px);[m
[32m+         box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
      }[m
  [m
      .tdd-action-btn:active:not(:disabled) {[m
[36m@@@ -422,7 -415,7 +678,11 @@@[m
  }[m
  [m
      .tdd-action-btn--primary:hover:not(:disabled) {[m
[32m++<<<<<<< HEAD[m
[32m +        background: var(--accent-primary-hover);[m
[32m++=======[m
[32m+         background: linear-gradient(135deg, var(--accent-primary-hover), #1d4ed8);[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
      }[m
  [m
  .tdd-action-btn--secondary {[m
[36m@@@ -432,7 -425,7 +692,11 @@@[m
  }[m
  [m
      .tdd-action-btn--secondary:hover:not(:disabled) {[m
[32m++<<<<<<< HEAD[m
[32m +        background: var(--bg-surface-3);[m
[32m++=======[m
[32m+         background: var(--border-color);[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
          color: var(--text-primary);[m
      }[m
  [m
[36m@@@ -466,8 -471,8 +742,13 @@@[m
  .tdd-tabs {[m
      display: flex;[m
      flex-wrap: wrap;[m
[32m++<<<<<<< HEAD[m
[32m +    gap: 2px;[m
[32m +    padding: 4px;[m
[32m++=======[m
[32m+     gap: 4px;[m
[32m+     padding: var(--sp-2);[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
      background: var(--bg-surface-2);[m
      border: 1px solid var(--border-color);[m
      border-radius: var(--r-md);[m
[36m@@@ -478,30 -483,31 +759,49 @@@[m
  .tdd-tab {[m
      padding: var(--sp-2) var(--sp-5);[m
      font-family: var(--font-body);[m
[32m++<<<<<<< HEAD[m
[32m +    font-size: 13.5px;[m
[32m +    font-weight: 600;[m
[32m++=======[m
[32m+     font-size: 14px;[m
[32m+     font-weight: 700;[m
[32m++>>>>>>> 3c9b04c268d726151b1152af403e9d9b7364d522[m
      color: var(--text-muted);[m
      background: transparent;[m
      border: none;[m
      border-radius: var(--r-sm);[m
      cursor: pointer;[m
[32m++<<<<<<< HEAD[m
[32m +    transition: color var(--t-fast), background var(--t-fast);[m
[32m++=