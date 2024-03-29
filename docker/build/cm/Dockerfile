# escape=`


# Increment this to invalidate the Docker layer cache for all downstream steps.
# This can be used to force automatic build systems to build from scratch.
ARG CACHE_BUSTER=1

# Main arguments
ARG BASE_IMAGE
ARG SPE_IMAGE
ARG TOOLING_IMAGE
ARG SOLUTION_IMAGE
ARG MANAGEMENT_SERVICES_IMAGE

FROM ${SOLUTION_IMAGE} as solution
FROM ${TOOLING_IMAGE} as tooling
FROM ${MANAGEMENT_SERVICES_IMAGE} AS management_services
FROM ${SPE_IMAGE} as spe
FROM ${BASE_IMAGE}

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Copy developer tools and entrypoint
COPY --from=tooling C:\tools C:\tools

WORKDIR C:\\inetpub\\wwwroot

# Copy the Sitecore Powershell Extensions Module
COPY --from=spe C:\module\cm\content .\

# Copy the Sitecore Management Services Module
COPY --from=management_services C:\module\cm\content C:\inetpub\wwwroot

# Copy solution website files
COPY --from=solution \artifacts\website\ .\
