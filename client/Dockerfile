FROM node:12-alpine

MAINTAINER Igor Agapie <igoragapie@gmail.com>

RUN set -ex \
	&& apk add --no-cache --virtual .persistent-deps \
		tini \
		nano \
		git \
		shadow \
		make \
		g++ \
		gcc \
		libgcc \
		libstdc++ \
		linux-headers \
		python \
		gnupg \
		curl

RUN set -ex \
	&& npm i -g @angular/cli

RUN set -eux; \
	mkdir -p /var/www/{app,client}

COPY . /var/www/app/

WORKDIR /var/www/app

RUN set -ex \
	&& npm i \
	&& ng b --aot="true" --prod="true" --outputPath="/var/www/client"
