FROM mono:3.12.1

RUN apt-get update
RUN apt-get install vim git zip -y
RUN apt-get clean && rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

RUN mkdir -p /build/output

COPY ./build/start.sh /build/start.sh
RUN chmod 777 /build/start.sh

ENTRYPOINT "/build/start.sh"
