# escape=`
# todo: Docker: validate this whole file

# Increment this to invalidate the Docker layer cache for all downstream steps.
# This can be used to force automatic build systems to build from scratch.
ARG CACHE_BUSTER=1

ARG BASE_IMAGE
ARG BUILD_IMAGE
ARG BUILD_CONFIGURATION

FROM ${BUILD_IMAGE} AS prep

# Gather only artifacts necessary for NuGet restore, retaining directory structure
COPY src\*.sln nuget.config *.targets *.props \nuget\
RUN echo hack
COPY src\ \temp\

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]
RUN Invoke-Expression 'robocopy C:\temp C:\nuget\src /s /ndl /njh /njs *.csproj *.scproj packages.config'

FROM ${BUILD_IMAGE} AS builder

ARG BUILD_CONFIGURATION

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Create an empty working directory
WORKDIR C:\build

# Copy prepped NuGet artifacts, and restore as distinct layer to take better advantage of caching
COPY --from=prep .\nuget .\
RUN echo hack
# Copy remaining source code
COPY src\ .\src\

RUN nuget restore .\src\DreamTeam.sln
# Copy transforms, retaining directory structure
RUN Invoke-Expression 'robocopy C:\build\src C:\out\transforms /s /ndl /njh /njs *.xdt'

# todo: validate this whole file
# Build website with file publish
RUN msbuild .\src\DreamTeam.sln /p:Configuration=$env:BUILD_CONFIGURATION /p:DeployOnBuild=True /p:DeployDefaultTarget=WebPublish /p:WebPublishMethod=FileSystem /p:PublishUrl=C:\out\website

FROM ${BASE_IMAGE}

WORKDIR C:\artifacts

# Copy final build artifacts
COPY --from=builder C:\out\website .\website\
RUN echo hack

COPY --from=builder C:\out\transforms .\transforms\
