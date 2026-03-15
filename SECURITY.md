# Security & Branch Setup

## Security PIN

The application is protected by a security PIN that must be entered before accessing the branch selection and check-in screens.

### Default PIN: `1234`

**To change the PIN:**
1. Open `Services/ISecurityService.cs`
2. Modify the `SECURITY_PIN` constant in `MockSecurityService`
3. In production, this should be loaded from secure configuration (environment variables, Azure Key Vault, etc.)

## Branch Management

### Automatic Branch Loading

The system automatically fetches branch information from the North End Coffee website: `https://www.northendcoffee.com/cafes`

**Current Branches** (fetched from website):
- Borak Mehnur | Banani
- Concord Baksh Tower | Gulshan-2
- Lotus Kamal Tower 2 | Gulshan-1
- Cityscape Tower | Gulshan Avenue
- Ahmed & Kazi Tower, 3rd Floor | Dhanmondi
- Shahabuddin Ahmed Park | Gulshan-2
- Roastery & Café | Tejgaon
- City Bank Center | Gulshan-1
- Ascott Palace | Baridhara
- Evercare Hospital | Bashundhara
- Alamin Bhaban | Ukhiya
- Liberty Tower, Ground Floor | Uttara
- Liberty Tower, 3rd Floor | Uttara
- Ahmed & Kazi Tower, Ground Floor | Dhanmondi

### Fallback Data

If the website fetch fails, the system uses hardcoded branch data from the website as fallback.

##Usage Flow

1. **Security PIN Entry** → Enter PIN (default: 1234) → Click "Next →"
2. **Branch Selection** → Choose your branch location
3. **Check-In System** → Enter Employee ID → Take Photo → Confirm Check-In/Out

## Services

### AppStateService
- **Purpose**: Manages authentication state and selected branch
- **Scope**: Singleton
- **State**: `IsAuthenticated`, `SelectedBranch`

### ISecurityService
- **Purpose**: Validates security PIN
- **Implementation**: MockSecurityService with hardcoded PIN
- **Methods**: `ValidatePin(string pin)`, `GetConfiguredPin()`

### IBranchService
- **Purpose**: Fetches branch data from North End Coffee website
- **Implementation**: MockBranchService with HTTP client
- **Methods**: `GetBranchesAsync()`, `GetBranchByIdAsync(int id)`
- **Caching**: Branches are cached after first successful fetch

##Production Considerations

1. **Security PIN**: Move to secure configuration management
2. **Branch API**: Consider creating dedicated API endpoint instead of web scraping
3. **Authentication**: Implement proper authentication/authorization if needed
4. **Session Management**: Add session timeout and re-authentication
5 **Audit Logging**: Log security PIN attempts and branch selections
