## Estimating Priority

### Priority Scale (MoSCoW)

- **Must Have**
  - On the critical path
- **Should Have**
  - Important, but not critical
- **Could Have**
  - Nice to have
- **Won't Have**
  - Not required

## Estimating Effort

### Areas of Effort

While there should be one overall rating for the item/issue overall,
where specific areas of imbalance in the effort are identified,
the rationale for the rating should call out the specific areas of effort.

These individual areas of concern may be used to help identify
areas of risk, and to help identify areas where additional
effort may be required.

- **Implementation effort**
  - e.g., coding, configuration, etc.
- **Data validation effort**
  - e.g., sourcing/creating/generating sample data, data migrations, data cleanup, etc.
- **QA effort**
  - e.g., test automation, manual testing, etc.
- **User acceptance effort**
  - e.g., training, documentation, specific areas of research, above-normal etc.
- **Deployment effort**
  - e.g., database migrations, approvals from other teams, CAB approval, etc.
- **Coordination effort**
  - e.g., coordination with other teams, coordination with 3rd-parties, etc.
- **Specialist effort**
  - e.g., security review, accessibility review, etc.
- **Documentation effort**
  - e.g., updating documentation, creating new documentation, gaining approval for documentation, etc.
- **Other effort**
  - e.g., other areas of effort not covered above

### Scale:

- **Trivial / Effortless**
  - Description:
    - Something which needs to be done, but is so simple that it's not worth estimating
    - These things might not even be worth recording as issues, but may be recorded
      somewhere so that they can be tracked and some historical record is kept
  - Examples:
    - fixing a typo, simple updates to style, etc.
- **Simple**
  - Description:
    - Something which is straightforward and easy to implement
    - Work that could potentially be self-approved with only limited review
  - Examples:
    - creating a new page with simple content, running of automated tooling
      (e.g., axe, lighthouse) with little expected follow-up work required, etc.
- **Routine**
  - Description:
    - Something which is straightforward, but may require some additional effort and/or coordination
    - Work that would undergo normal review and approval processes
  - Examples:
    - creating a new page with typical content, creating a new API endpoint,
      submitting requests to external teams, sourcing routine user research participants,
      coordination with another team (e.g., requests via ServiceNow),
      updating deployment configuration, etc.
- **Tricky/Awkward/Elevated Risk - Requires some additional care and/or effort**
  - Description:
    - Something which is not straightforward, but is still within the team's capabilities
    - Should be on the whole team's radar, not just the implementer(s)
    - If due to the size of the work, may benefit from splitting down into smaller items
    - Could be mostly routine, but the impact of getting it wrong may be high
      - Note: this is not the same as "Complex" (see below)
      - Note: this could include cases where the work is mostly straightforward,
        but the impact of getting it wrong is substantial
      - (e.g., accidentally exposing sensitive data, deleting data, deleting Azure resources)
    - Work which might require specific CAB approval, or additional up-front review
    - Identifiable as a particular risk where timeline/implementation is uncertain
      and/or not under the team's control
    - May benefit from additional specialist support and input (e.g., the IT Security team,
      the Accessibility team, (Data) Architecture team, etc.)
  - Examples:
    - creating a new API endpoint which requires a new database table,
      updating a large number of inter-related dependencies, substantial coordination/effort
      involving external teams, sourcing of user research participants of a very narrow profile,
      reversible data migration steps, etc.
- **Complex**
  - Description:
    - Something which is not straightforward, and/or requires substantial coordination,
      and/or is outside the team's capabilities to implement in one go
    - Work which is extremely broad, in need of refinement, and needs to be split into smaller items
    - Work which needs substantial specialist input, perhaps from a 3rd-party
  - NOTE: While the _implementation_ may be complex, the _co-ordination_ and _organising_
    of this work may be more straightforward,
    - in which case the coordination and implementation tasks might be recorded separately
      (noting that their "done" states might be achieved independently)
  - Examples:
    - "Epic" or "Feature" level work, IT Health Check, Accessibility Audit, penetration testing,
      service assessment, irreversible substantial data migration steps, etc.

## Effort Multipliers

While the effort rating may be a single value describing how much effort/complexity is involved,
there may be additional factors which may mean completing this work is more difficult than
the effort rating alone would suggest.

e.g., a "Simple" effort rating may be given, but if the work is being done by a single person
who is new to the team/technology, then the effort rating may be multiplied to account for
the learning curve.

e.g., a "Routine" effort rating may be given, but if the work requires putting in a request to
another team, then the effort rating may be multiplied to account for the additional
coordination and time required.

- New/unfamiliar (to team) technology
- First time implementing this within this project
- Dependency on external (DfE internal) team
- Dependency no multiple external (DfE internal) teams
- Dependency on 3rd-party (DfE external) team(s)
- Multiple internal implementers

## Anticipated Impediments

Areas which will make work more complex/slow work down:

- People
  - Knowledge, skills, experience, etc.
- Process
  - Dependencies, approvals, etc.
- Technology
  - New/unfamiliar technology, etc.
- Scope
  - Size, complexity, etc.
- Unknowns
  - Risks, assumptions, etc.
- Communication
  - Coordination, etc.
- Other
  - Other areas of concern

Suggestion of template to record note about impediments:

- [ ] **Impediment:** [Description of impediment]
  - **Mitigation:** [Description of mitigation]
  - **Rationale:** [Description of rationale]
  - **Impact:** [Description of impact]
  - **Status:** [Description of status]
  - **Notes:** [Description of notes]

