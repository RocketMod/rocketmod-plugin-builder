FROM mono:3.12.1

RUN apt install vim mono-reference-assemblies-3.5
RUN apt-get clean && rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

CMD ['/bin/bash']
