FROM microsoft/aspnet

RUN apt-get install git -y
RUN mkdir /repos
WORKDIR /repos
RUN git clone https://github.com/catwithboots/Hortlak.git
WORKDIR /repos/Hortlak
RUN mkdir /root/.config
RUN mkdir /root/.config/NuGet
COPY .nuget/NuGet.Config /root/.config/NuGet/NuGet.Config
RUN nuget restore Hortlak.sln
RUN xbuild Hortlak.sln

EXPOSE 9000

CMD ["mono /repos/Hortlak/Service/bin/Debug/Hortlak.exe"]
