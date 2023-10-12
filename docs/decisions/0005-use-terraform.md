## Use Terraform

### Options

DfE standard is to use:

- Terraform
- Azure Resource Manager (ARM) templates

See also:

https://technical-guidance.education.gov.uk/guides/default-technology-stack/#infrastructure-as-code
> DfE uses Terraform and Azure Resource Manager (ARM) templates for automating and scripting Azure infrastructure
> creation and changes.

### Decision

We will use Terraform to manage our infrastructure as code.

Terraform modules being developed by DfE Digital for new projects, to have
standardised infrastructure components, and to make it easier to share
infrastructure components between projects.

- https://github.com/DFE-Digital/terraform-modules


