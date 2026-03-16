# North End Coffee - Employee Check-In System
## Project Specification Document

**Version:** 1.0  
**Last Updated:** March 16, 2026  
**Project Type:** Progressive Web Application (PWA)  
**Platform:** Blazor WebAssembly (.NET 8.0)

---

## Table of Contents
1. [Project Overview](#project-overview)
2. [Technology Stack](#technology-stack)
3. [System Architecture](#system-architecture)
4. [Features & Functionality](#features--functionality)
5. [User Interface Design](#user-interface-design)
6. [Security Implementation](#security-implementation)
7. [Deployment Configuration](#deployment-configuration)
8. [Responsive Design](#responsive-design)
9. [PWA Capabilities](#pwa-capabilities)
10. [API Integration Points](#api-integration-points)

---

## Project Overview

### Purpose
A modern, tablet-optimized Progressive Web Application for North End Coffee employee check-in/check-out system deployed across multiple branch locations.

### Goals
- Provide a professional kiosk-style entrance system for employee attendance tracking
- Support multiple branch locations with dedicated tablets per branch
- Enable photo-based check-in/check-out with facial verification
- Offer offline capability for continuous operation
- Maintain North End Coffee brand identity throughout the UX

### Target Devices
- **Primary:** iPad Mini, iPad (portrait and landscape)
- **Secondary:** Android tablets (7-10 inch)
- **Mobile:** iOS/Android smartphones (QR code-based entry)

---

## Technology Stack

### Frontend Framework
- **Blazor WebAssembly** (Client-side SPA)
- **.NET 8.0** Runtime
- **C#** Language

### Key Dependencies
```xml
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly" Version="8.0.24" />
<PackageReference Include="QRCoder" Version="1.7.0" />
```

### UI Technologies
- **CSS3** (Custom styling, no framework dependencies)
- **Flexbox & Grid** (Responsive layouts)
- **JavaScript Interop** (Camera access, localStorage)

### Deployment Platform
- **Azure Web Apps** (PaaS)
- **URL:** https://northendoneentrance.azurewebsites.net

---

## System Architecture

### Application Structure
```
NorthEndOneEntrance/
├── Pages/
│   ├── Home.razor              # Main application page
│   ├── Counter.razor           # Demo page
│   └── Weather.razor           # Demo page
├── Layout/
│   ├── MainLayout.razor        # App shell layout
│   └── NavMenu.razor           # Navigation component
├── Services/
│   ├── AppStateService.cs      # Global state management
│   ├── IBranchService.cs       # Branch data interface
│   ├── MockBranchService.cs    # Mock branch data provider
│   ├── IEmployeeCheckInService.cs    # Check-in interface
│   ├── MockEmployeeCheckInService.cs # Mock check-in service
│   ├── ISecurityService.cs     # Security interface
│   └── NetworkSyncService.cs   # Offline sync (future)
├── wwwroot/
│   ├── css/app.css             # Main stylesheet (~2000 lines)
│   ├── index.html              # App shell
│   ├── manifest.webmanifest    # PWA configuration
│   ├── service-worker.js       # Offline support
│   └── north-end-logo.png      # Brand logo
└── Program.cs                   # App startup configuration
```

### Service Layer

#### AppStateService (Singleton)
- Manages authentication state
- Handles branch selection persistence
- Provides global state change notifications
- Uses localStorage for state persistence

#### BranchService (Scoped)
- Retrieves available branch locations
- Supports branch-by-ID lookup
- Currently uses mock data (ready for API integration)

#### EmployeeCheckInService (Singleton)
- Validates employee IDs
- Processes check-in/check-out with photo capture
- Tracks employee status (checked-in/out)
- Maintains check-in history

#### SecurityService (Singleton)
- Validates 4-digit security PIN
- Controls access to branch selection

---

## Features & Functionality

### 1. Security Authentication
**Flow:** PIN Entry → Branch Selection → Check-In System

**Implementation:**
- 4-digit PIN verification screen
- Professional split-screen layout (brand colors left, PIN entry right)
- Auto-clear on invalid attempt
- Visual feedback with dot indicators

**PIN Screen Components:**
- Left Panel: Logo, Welcome message, Live clock, Date
- Right Panel: Security verification form, Number keypad (0-9), Delete button
- Submit button (enabled when 4 digits entered)

### 2. Branch Selection
**Flow:** Authentication → Multi-branch Grid → Branch Loaded

**Features:**
- Dynamic branch loading
- Card-based selection interface
- Branch details: Name, Location, Branch Code
- Hover animations and visual feedback
- URL-based routing (`/{BranchId}`)

**Design:**
- Clean, modern card grid
- Centered layout with North End Coffee branding
- Brand colors: Coffee brown (#7e3e28, #5d2e1f)
- Responsive grid (1-3 columns based on screen size)

### 3. Employee Check-In System
**Flow:** Enter ID → Capture Photo → Review → Confirm → Success

**Step 1: Employee ID Entry**
- Numeric keypad (0-9, Delete, Next)
- ID validation (minimum 4 digits)
- Employee lookup from service
- Auto-detection of check-in vs check-out state

**Step 2: Photo Capture**
- Live camera feed via WebRTC
- Full-screen camera interface
- Photo preview and retake option
- Facial photo requirement for verification

**Step 3: Confirmation**
- Display captured photo
- Show check-in/check-out action
- Retake or confirm options
- Timestamp recording

**Step 4: Success**
- Success confirmation screen
- Employee ID display
- Timestamp shown
- 3-second auto-reset to ID entry

### 4. Branch Information Panel
**Always Visible (Left Column):**
- North End Coffee logo
- Branch name/location
- Live digital clock (updates every second)
- QR code for mobile check-in
- Settings button (logout/change branch)

**QR Code Feature:**
- Generates branch-specific QR code
- Links to: https://www.northendcoffee.com/checkin
- Enables smartphone-based check-in
- White background with branded border

---

## User Interface Design

### Brand Identity
**Primary Colors:**
- Coffee Brown: `#7e3e28`
- Dark Brown: `#5d2e1f`
- Cream: `#f5f0eb`
- Light Beige: `#f9f6f3`

**Typography:**
- System Fonts: Helvetica Neue, Helvetica, Arial, sans-serif
- Monospace (Clock): Courier New

**Design Principles:**
- Clean, professional aesthetic
- Coffee shop warmth through gradients
- High contrast for readability
- Generous white space
- Touch-optimized button sizes

### Screen Layouts

#### 1. PIN Entry Screen (auth-screen-v2)
```
┌─────────────────────────────────────────────┐
│  LEFT PANEL (50%)   │  RIGHT PANEL (50%)    │
│  - Brown gradient   │  - White background   │
│  - Logo (200px)     │  - PIN dots           │
│  - Welcome text     │  - Number keypad      │
│  - Tagline          │  - Submit button      │
│  - Clock (2.25rem)  │                       │
│  - Date             │                       │
│  max-width: 480px   │  max-width: 480px     │
└─────────────────────────────────────────────┘
```

#### 2. Branch Selection Screen
```
┌──────────────────────────────────────────────┐
│           North End Coffee Logo              │
│        "Welcome to North End"                │
│     "Select your branch to continue"         │
│                                              │
│  ┌────────┐  ┌────────┐  ┌────────┐        │
│  │Branch 1│  │Branch 2│  │Branch 3│        │
│  │Location│  │Location│  │Location│        │
│  │ [CODE] │  │ [CODE] │  │ [CODE] │        │
│  └────────┘  └────────┘  └────────┘        │
│                                              │
└──────────────────────────────────────────────┘
```

#### 3. Check-In System
```
┌─────────────────────────────────────────────┐
│  LEFT COLUMN        │  RIGHT COLUMN          │
│  - Logo             │  - Step indicator      │
│  - Branch name      │  - ID input display    │
│  - Digital clock    │  - Keypad (3x4 grid)   │
│  - QR code          │  - Status messages     │
│  - QR label         │  OR                    │
│                     │  - Camera view         │
│                     │  - Photo preview       │
└─────────────────────────────────────────────┘
```

### UI Components Inventory

**Buttons:**
- `.num-key` - Number keypad buttons (72px height, rounded)
- `.key-btn` - Check-in keypad (larger, touch-optimized)
- `.auth-submit-btn` - Primary action button
- `.branch-card` - Branch selection cards
- `.action-btn` - Camera action buttons

**Input Displays:**
- `.pin-dot` - PIN entry indicators (4 dots)
- `.id-display` - Employee ID display
- `.digital-clock` - Live time display

**Containers:**
- `.auth-screen-v2` - PIN entry layout
- `.branch-selection-screen` - Branch grid
- `.check-in-container` - Main check-in interface
- `.main-layout` - Two-column split layout

---

## Security Implementation

### Authentication Flow
1. **PIN Verification**
   - Stored securely in SecurityService
   - No persistence (session-based)
   - Invalid attempts clear input

2. **State Persistence**
   - Authentication state in localStorage
   - Branch selection persisted across sessions
   - Auto-load on page refresh

3. **Session Management**
   - Logout clears all state
   - Returns to PIN entry
   - Clears URL parameters

### Camera Security
- User permission required
- Camera stops on navigation away
- No photo storage on device
- Photos transmitted for verification only

---

## Deployment Configuration

### Azure Web App Setup
- **Region:** Azure-selected
- **Runtime:** .NET 8.0
- **Deployment Method:** Kudu ZIP API
- **Endpoint:** `/api/zip/site/wwwroot/`

### Build Process
```powershell
# Build for production
dotnet publish -c Release -o ./publish

# Create deployment package
Compress-Archive -Path ./publish/wwwroot/* -DestinationPath ./deploy.zip -Force

# Deploy via Kudu API
Invoke-RestMethod -Uri 'https://northendoneentrance.scm.azurewebsites.net/api/zip/site/wwwroot/' `
  -Headers @{Authorization="Basic [credentials]"} -Method Put -InFile './deploy.zip'
```

### File Integrity
- **SRI (Subresource Integrity)** enabled
- SHA-256 hashes for .wasm, .dat, .json files
- Binary file preservation via ZIP deployment

### MIME Types (web.config)
```xml
<mimeMap fileExtension=".webmanifest" mimeType="application/manifest+json" />
<mimeMap fileExtension=".dat" mimeType="application/octet-stream" />
<mimeMap fileExtension=".wasm" mimeType="application/wasm" />
<mimeMap fileExtension=".json" mimeType="application/json" />
```

---

## Responsive Design

### Breakpoints Strategy

#### Mobile Portrait (< 640px)
- Single column layout
- Stacked PIN panels
- Logo: 180px
- Reduced font sizes
- Full-width branch cards

#### Mobile Landscape (641px - 767px)
- Two-column PIN layout
- Compact spacing
- Logo: 180px

#### Tablet Portrait (768px - 834px) - iPad Mini
- Optimized spacing
- Logo: 180px
- Branch title: 1.85rem
- QR code: 150px
- Two-column layout maintained

#### Tablet Landscape (835px - 1024px)
- Full two-column layouts
- Logo: 220px
- Optimal spacing
- 2-3 branch cards per row

#### Desktop (> 1024px)
- Left column: 40% width
- Right column: 60% width
- Maximum component sizes
- 3+ branch cards per row

### Touch Optimization
```css
touch-action: pan-x pan-y;
-webkit-user-select: none;
user-select: none;
-webkit-tap-highlight-color: transparent;
```

### Viewport Configuration
```html
<meta name="viewport" content="width=device-width, initial-scale=1.0, 
      maximum-scale=1.0, user-scalable=no, interactive-widget=resizes-visual" />
```

**Features:**
- Zoom completely disabled
- Touch scrolling only
- No text size adjustment
- Interactive widget compatibility

---

## PWA Capabilities

### Progressive Web App Features

#### Installability
- Add to Home Screen (iOS/Android)
- Standalone display mode
- Custom app icons (512x512, 192x192)
- Splash screen configuration

#### Offline Support
- Service Worker implementation
- Asset caching strategy
- Background sync capability
- Network-first with fallback

#### Manifest Configuration
```json
{
  "name": "North End Coffee - Check In",
  "short_name": "North End Check In",
  "display": "standalone",
  "theme_color": "#7e3e28",
  "background_color": "#f5f0eb"
}
```

#### Installation Benefits
- Full-screen kiosk mode
- No browser UI distraction
- App-like experience
- Isolated session
- Faster loading (cached assets)

### Service Worker Strategy
- Cache-first for static assets
- Network-first for API calls
- Automatic updates on deploy
- Version management

---

## API Integration Points

### Current Implementation: Mock Services

#### IBranchService
```csharp
Task<List<Branch>> GetBranchesAsync();
Task<Branch?> GetBranchByIdAsync(int branchId);
```

**Mock Data Structure:**
```csharp
public class Branch {
    int Id;
    string Name;        // "Gulshan - DCC Market"
    string Location;    // "Ahmed & Kazi Tower, Ground Floor"
    string Code;        // "GLS-001"
}
```

**Current Mock Branches:**
1. Gulshan - DCC Market (GLS-001)
2. Banani - Kamal Ataturk (BAN-002)
3. Dhanmondi - Satmasjid Road (DHN-003)
4. Uttara - Sector 7 (UTR-004)

#### IEmployeeCheckInService
```csharp
bool ValidateEmployeeId(string employeeId);
Task<EmployeeStatus> GetEmployeeStatusAsync(string employeeId);
Task<CheckInResult> ProcessCheckInAsync(string employeeId, string photoBase64);
```

**Mock Employees:**
- Employee IDs: 1001-1020
- Auto check-in/out toggle
- Photo capture support

#### ISecurityService
```csharp
bool ValidatePin(string pin);
```

**Default PIN:** `1234`

### Future Production Integration

#### API Endpoints (Ready for Implementation)
```
GET  /api/branches              → List all branches
GET  /api/branches/{id}         → Get branch details
POST /api/auth/validate-pin     → Validate security PIN
GET  /api/employees/{id}/status → Get employee status
POST /api/checkin               → Process check-in/out
```

**Required Request/Response Models:**
- BranchListResponse
- EmployeeStatusResponse
- CheckInRequest (with Base64 photo)
- CheckInResponse (with timestamp)

---

## Camera Integration

### JavaScript Interop Functions

```javascript
// Initialize camera
window.initCamera = async function() {
    // Request camera permission
    // Start video stream
    // Return error message or empty string
}

// Capture photo
window.takePhoto = async function() {
    // Capture frame from video
    // Convert to Base64 JPEG
    // Return DataURL
}

// Stop camera
window.stopCamera = function() {
    // Stop all tracks
    // Release camera resources
}
```

### Implementation Details
- **getUserMedia API** for camera access
- **Canvas API** for photo capture
- **Base64 encoding** for transmission
- **Auto-stop** on navigation away
- **Error handling** for permission denial

---

## Performance Considerations

### Bundle Size Optimization
- Lazy loading for routes (future)
- Service Worker caching
- Compression enabled (.br files)
- Minimal dependencies

### Loading Strategy
- Skeleton loading screen
- Progressive rendering
- Asset preloading
- Critical CSS inline (future optimization)

### State Management
- Singleton services for shared state
- LocalStorage persistence
- Minimal re-renders
- Event-based updates

---

## Testing Requirements

### Manual Testing Checklist

#### Authentication Flow
- [ ] PIN entry accepts 4-6 digits
- [ ] Invalid PIN shows error message
- [ ] Valid PIN proceeds to branch selection
- [ ] State persists on page refresh

#### Branch Selection
- [ ] All branches load correctly
- [ ] Branch cards are clickable
- [ ] Selection navigates with URL parameter
- [ ] Branch persists on reload

#### Check-In Flow
- [ ] Employee ID validation works
- [ ] Camera permissions requested
- [ ] Photo capture produces clear image
- [ ] Check-in/out status updates
- [ ] Success screen auto-resets

#### Responsive Design
- [ ] Works on iPad Mini portrait
- [ ] Works on iPad Mini landscape
- [ ] Works on iPad Pro
- [ ] Works on mobile devices
- [ ] No zoom on double-tap

#### PWA Features
- [ ] Installable on iOS
- [ ] Installable on Android
- [ ] Works offline (cached assets)
- [ ] App icon shows correctly
- [ ] Runs in standalone mode

---

## Deployment Checklist

### Pre-Deployment
- [ ] Update version number
- [ ] Test all features locally
- [ ] Verify responsive design
- [ ] Check camera permissions
- [ ] Test on target devices

### Build Process
- [ ] Run `dotnet publish -c Release`
- [ ] Verify no build errors
- [ ] Check bundle size
- [ ] Test published output locally

### Deployment
- [ ] Create deployment ZIP
- [ ] Deploy via Kudu API
- [ ] Verify deployment success
- [ ] Check file integrity (SRI)
- [ ] Test MIME types

### Post-Deployment
- [ ] Test live URL
- [ ] Verify PWA installability
- [ ] Check all features work
- [ ] Test on actual tablets
- [ ] Monitor for errors

---

## Future Enhancements

### Phase 2 Features
1. **Backend API Integration**
   - Replace mock services with real API calls
   - Implement authentication tokens
   - Add network retry logic

2. **Enhanced Security**
   - Multi-level PIN configuration
   - Branch-specific PINs
   - Admin dashboard

3. **Reporting & Analytics**
   - Daily attendance reports
   - Employee check-in history
   - Branch-wise analytics

4. **Offline Sync**
   - Queue check-ins when offline
   - Sync when connection restored
   - Conflict resolution

5. **Notifications**
   - Late arrival alerts
   - Missing check-out reminders
   - Branch-specific announcements

### Technical Improvements
- Lazy loading optimization
- Image compression before upload
- Internationalization (i18n)
- Accessibility improvements (WCAG 2.1)
- Automated testing suite

---

## Support & Maintenance

### Browser Requirements
- **iOS Safari:** 14+
- **Chrome/Edge:** 90+
- **Android WebView:** 90+

### Known Limitations
- Camera requires HTTPS
- iOS requires user interaction for camera
- Service Worker requires secure context
- LocalStorage limited to 5-10MB

### Troubleshooting

#### Camera Not Working
1. Check HTTPS connection
2. Verify browser permissions
3. Test on different browser
4. Check device camera availability

#### PWA Not Installing
1. Verify manifest.webmanifest accessible
2. Check service worker registration
3. Ensure HTTPS enabled
4. Clear browser cache

#### Deployment Issues
1. Verify Kudu credentials
2. Check file integrity (SRI)
3. Validate web.config MIME types
4. Test with fresh browser cache

---

## Appendix

### Project Statistics
- **Total CSS Lines:** ~2000
- **Total Components:** 15+
- **Services:** 6
- **Supported Devices:** 10+
- **Breakpoints:** 5 major

### Development Timeline
- **Phase 1:** Core functionality (Completed)
- **Phase 2:** UI polish & responsive design (Completed)
- **Phase 3:** PWA optimization (Completed)
- **Phase 4:** API integration (Pending)

### Contact & Resources
- **Repository:** adyrin-cloud/NorthEndOneEntrance
- **Branch:** main
- **Deployment:** Azure Web Apps
- **Documentation:** This spec file

---

**Document End**

*This specification document serves as the single source of truth for the North End Coffee Employee Check-In System project. All development, testing, and deployment activities should reference this document.*
