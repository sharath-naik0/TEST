# Use the official .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
 
# Set the working directory inside the container
WORKDIR /workspace
 
# Install required packages for Selenium and Chrome
RUN apt-get update && apt-get install -y \
    wget \
    unzip \
    curl \
    gnupg2 \
    # Install Google Chrome
&& echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" > /etc/apt/sources.list.d/google-chrome.list \
&& wget -q -O - https://dl.google.com/linux/linux_signing_key.pub | apt-key add - \
&& apt-get update && apt-get install -y google-chrome-stable \
    # Install ChromeDriver
&& CHROME_VERSION=$(google-chrome --version | grep -o '[0-9]*\.[0-9]*\.[0-9]*') \
&& CHROMEDRIVER_VERSION=$(curl -sS https://chromedriver.storage.googleapis.com/LATEST_RELEASE_$CHROME_VERSION) \
&& wget -N https://chromedriver.storage.googleapis.com/$CHROMEDRIVER_VERSION/chromedriver_linux64.zip \
&& unzip chromedriver_linux64.zip -d /usr/local/bin/ \
&& chmod +x /usr/local/bin/chromedriver \
&& rm chromedriver_linux64.zip \
&& apt-get clean \
&& rm -rf /var/lib/apt/lists/*
 
# Copy the project files into the container
COPY . .
 
# Restore dependencies and build the project
RUN dotnet restore
RUN dotnet build --configuration Release
 
# Set the entry point to run your tests (adjust the project name as needed)
CMD ["dotnet", "test", "--configuration", "Release", "--no-build", "--verbosity", "normal"]
