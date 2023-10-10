<!-- 
    Template adapted from the short version example: https://adr.github.io/madr/examples.html
    Used under CC0 license (does not require attribution etc., but included here as good practice).

    Note that this template is likely to evolve over time as we gain experience with it and decide what works best.
-->

# Use Markdown Any Decision Records (MADRs)

## Context and Problem Statement

_NOTE: This decision record and template is adapted from:_
https://github.com/adr/madr/blob/develop/docs/decisions/0000-use-markdown-any-decision-records.md?plain=1

<!-- Brief summary in a handful of sentences. -->
<!-- Use whatever format/structure makes sense at the time - placeholders below are just a suggestion. -->

- We want to record any decisions made in this project independent whether decisions concern the architecture ("architectural decision record"), the code, or any other .
- Which format and structure should these records follow?


### Related decisions:

- N/A


## Considered Options

<!-- Variable length list of options considered. Not doing an exhaustive search is okay, just say so. -->

- [MADR](https://adr.github.io/madr/) 3.0.0 – The Markdown Any Decision Records
- [Michael Nygard's template](http://thinkrelevance.com/blog/2011/11/15/documenting-architecture-decisions) – The first incarnation of the term "ADR"
- [Sustainable Architectural Decisions](https://www.infoq.com/articles/sustainable-architectural-design-decisions) – The Y-Statements
- Other templates listed at <https://github.com/joelparkerhenderson/architecture_decision_record>
- Formless – No conventions for file format and structure



## Decision Outcome

Chosen option: "MADR 3.0.0", because:

- Implicit assumptions should be made explicit.
  Design documentation is important to enable people understanding the decisions later on.
  See also [A rational design process: How and why to fake it](https://doi.org/10.1109/TSE.1986.6312940).
- MADR allows for structured capturing of any decision.
- The MADR format is lean and fits our development style.
- The MADR structure is comprehensible and facilitates usage & maintenance.
- The MADR project is vivid.


## Decision Type / Level of Team Scrutiny

- [X] Log of independent decision *(e.g., adoption of department/team/profession standard, no consultation/discussion needed)*
- [ ] Log of informal decision between two or more team members *(e.g., during pair-programming session)*
- [ ] Log of formal work *(e.g., explicit spike)*
- [ ] Log of formal team consultation/decision process *(e.g., retrospective outcome)*


## Relevant/applicable service standards _(non-exhaustive)_:

- [X] [Standard point 11 - Choose the right tools and technology](https://apply-the-service-standard.education.gov.uk/service-standard/11-choose-the-right-tools-and-technology)
  - [GOV.UK Service Manual - Point 11](https://www.gov.uk/service-manual/service-standard/point-11-choose-the-right-tools-and-technology):
    - > be able to show that they’ve made good decisions about what technology to build and what to buy
  - [DfE Apply the Standard - Point 11](https://apply-the-service-standard.education.gov.uk/service-standard/11-choose-the-right-tools-and-technology):
    - > evidence that the team have considered different options to how the service will be delivered technically and the rationale for the chosen technical direction
    - > evidence that the team will continue to re-evaluate and challenge previous decisions as new requirements are discovered 
  - [DfE Architecture](https://dfe-digital.github.io/architecture/standards/architecture-documentation/#architecture-documentation):
    - > architectural artefacts, decisions (opens in new tab) and plans have been documented, shared across your community and reviewed and assured
- [X] [Standard point 13 - Use and contribute to open standards, common components and patterns](https://apply-the-service-standard.education.gov.uk/service-standard/13-use-common-standards-components-patterns)
  - MADR is the preferred DfE option within the DfE architecture community.
    > In DfE, we favour the Markdown Architecture Decision Records (MADR) approach, though appreciate any implementation is better than none.
- [X] [Standard point 14 - Operate a reliable service](https://apply-the-service-standard.education.gov.uk/service-standard/14-operate-a-reliable-service)


## Change History

*See git commit history for merge timings and full diffs.*

Human notes:
- Initial write-up based on https://github.com/adr/madr/blob/develop/docs/decisions/0000-use-markdown-any-decision-records.md?plain=1

