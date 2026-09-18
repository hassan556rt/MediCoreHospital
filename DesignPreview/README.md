# MediCore Design Preview

This directory contains a design-preview-only mockup set for the MediCore Hospital Management System.

## Purpose

These screens are intentionally created outside the production application and do not modify any app logic, WPF XAML, ViewModels, services, repositories, database code, or business logic.

This preview is for visual approval only.

## Design System

### Core identity
- Premium private hospital software
- Arabic-first RTL layout
- Modern Windows desktop aesthetic
- Medical brand palette built around navy, blue, teal, and cyan accents
- Clean, professional presentation for clinical operations and admin management

### Color palette
- Navy / deep blue: background, sidebar, modern admin shell
- Blue: primary actions, key metric cells, data emphasis
- Cyan / teal: health and status accents, dashboard charts, active states
- White / soft gray: content surfaces and light theme panels
- Semantic colors:
  - Success: green
  - Warning: amber
  - Danger: red

### Typography
- WPF-friendly sans serif styling with a strong Windows desktop look
- Arabic-first presentation with right-to-left flow
- Emphasis on hierarchy, spacing, and readability for clinical data
- Intended to map to WPF font settings such as Segoe UI, Cairo, or Tajawal when implemented later

### Navigation concept
- Fixed left sidebar for primary modules
- High-contrast top header for global actions and authenticated user context
- Cards and tables for operational dashboards and management screens
- Contextual actions grouped near headers for clean user interaction

### Components
- KPI cards for summary metrics
- Search/filter inputs for patients, appointments, and pharmacy inventory
- DataGrid-style tables with status pills and action buttons
- Chart blocks for visits, emergencies, and operational trends
- Rounded cards, subtle shadows, consistent spacing, and clear hierarchy

### Light / dark theme concept
- Light mode is the base clinical design for everyday professional use
- Dark mode is included as a visual system treatment for later implementation, not as a production change
- The same design language remains consistent across both themes

### WPF mapping guidance
These mockups map naturally to the following WPF concepts:
- Window / UserControl shell
- Border, Grid, StackPanel, DockPanel
- DataGrid for tables
- Cards built from Border + Grid layouts
- Buttons and icon buttons with rounded style treatment
- ResourceDictionary theme colors and brushes
- RTL FlowDirection set to RightToLeft

## Included previews

- Dashboard
- Patients Management
- Appointments
- Inpatient / Admissions
- Pharmacy
- Emergency

## Files
- `DesignPreview/index.html` — visual design preview set

## Approval gate

These files are intentionally preview-only and must not be treated as the final application implementation.

No production code has been modified.

The preview must be explicitly approved before implementation begins.
