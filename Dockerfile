FROM mono:5.20

RUN apt-get update
RUN apt-get install vim git zip -y
RUN apt-get clean && rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

RUN mkdir -p /build/output

COPY ./build/* /build/

RUN chmod 777 /build/start.sh

ENTRYPOINT "/build/start.sh"
