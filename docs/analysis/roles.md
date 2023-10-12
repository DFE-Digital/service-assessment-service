## System Roles

- **Super-User**
  - Role via AD, granted via ServiceNow request
  - Permissions/abilities:
    - full access to all systems and ability to assign all roles including System Roles
- **Developer/Operations/Engineer**
  - Role via AD, granted via ServiceNow request
  - Permissions/abilities:
    - access to diagnostic/system information 
      - (increased above coordinator? what should coordinator see?)7
      - e.g., non-public logs, metrics, etc.
    - limited access to personal data
    - similar to "Super-User" but with limited access to personal data
    - similar to "Coordinator" but with limited access to personal data
- **User(Role?) Impersonator**
  - Role via AD
    - Granted via Azure PIM?
    - Granted via "sudo" functionality to temporarily increase permissions?
    - Nuance re: if this is not a role, but a temporary elevation (change?) of permissions?
  - Permissions/abilities:
    - ability to "impersonate" users (e.g., to assist with development and debugging of production issues)
      - NOTE: all actions still recorded against actual user etc.
      - could make this a separate role?
      - limited access to personal data?
      - perhaps make more generic as "view as <role>" functionality that is also open to other roles?
        - e.g., delivery manager might want to "view as panel member" to see what they see / assist with local training
        - e.g., coordinator might want "view as panel member" to assist with training or assist with support via screen
          share without risking showing more than they "should"
      - perhaps ability to switch between "personas"?
        - at logon, or via a "switch persona" button?
        - at logon, presented with a list of personas to choose from for this session?
        - relevant where user might be logging in as a panel member (thus have additional view/edit permissions), but
          is also part of a delivery team being assessed and during those meetings may wish to login with delivery team
          role permissions during e.g., a group meeting (removes/eliminates risk of accidentally sharing too much
          information such as a pending decision on another project, as opposed to relying on the user being careful
          about what they show/share)
- **User Manager**
  - A "sudo" type role that allows elevation of permissions for a limited time (e.g., 1 hour)
- **Coordinator ("Administrator")**
  - Role via AD, granted via ServiceNow request
  - Permissions/abilities:
    - ability to invite new users(?)
      - do we require user to login first?
      - pre-granting permissions to a user?
      - latter feels awkward for beta - consider it an option for "later"
      - limit this to a "sudo" console where temporary elevation of permissions is granted (e.g., 1 hour) including
        associated enhanced logging / audit trail
    - ability to assign _project roles_ to (existing?) users
    - ability to assign _panel roles_ to (existing?) users
    - ability to view+edit all project information
      - normal "day to day" functionality vs "sudo"/elevated rights functionality?
    - TBD (see notes above): ability to "impersonate" users/roles (e.g., to assist with training and support)

## Project Roles

- **Deputy Director**
  - e.g., ability to view(+edit?) all project information (limited to scope of their directorate only)
- **Delivery Manager**
- **Project Manager**

## Panel Roles

https://apply-the-service-standard.education.gov.uk/service-assessments#who-is-on-the-assessment-panel

- **Lead Assessor**
- **User Researcher Assessor**
- **Design Assessor**
- **Technical Assessor**
- **Observer**

## Viewer Roles (not directly involved with an assessment)

- **Public, unauthenticated user**
- **Authenticated DfE User**
  - Internal User (@education.gov.uk, @digital.education.gov.uk)
  - External User
    - e.g., contractors
- **Authenticated Gov.UK User**
  - Internal User (@gov.uk, @digital.cabinet-office.gov.uk, etc.)
    - e.g., potential cross-gov assessors, cross-gov panel members, cross-gov observers
  - External User
    - e.g., contractors
- **Authenticated non-Gov.UK User**
  - e.g., login using a personal account
  - e.g., login using a work account with unrecognised domain

