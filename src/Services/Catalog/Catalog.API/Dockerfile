#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
# basically it is using multi-layered Docker structure and 


# the first layer is a base image
# the base image is asp.net 8 which sets up the runtime 
# environment 
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# and after that we are performing the build operation in 
# this line of code. 
# so here we restore and build our project and notice that how it references
# both catalog.api and building blocks. 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Catalog/Catalog.API/Catalog.API.csproj", "Services/Catalog/Catalog.API/"]
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
RUN dotnet restore "./Services/Catalog/Catalog.API/Catalog.API.csproj"
COPY . .
WORKDIR "/src/Services/Catalog/Catalog.API"
RUN dotnet build "./Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# so after that we can perform the publish operation in here. 
# at this stage we compile our application into a release build and ready
# for the deployment. 
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# and the final stage we can see that we set up the final image and copying 
# the published application and setting up the entrypoint in order to run 
# our application. 
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]

# the important part of the newly generated Docker file is inclusion 
# of the building blocks in our docker file.
# this is crucial as it ensures all dependecies are correctly build 
# into our container image. 