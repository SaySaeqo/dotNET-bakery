FROM node:14
WORKDIR /app
COPY package*.json ./
RUN npm install
RUN npm install mqtt --save
COPY . .
CMD [ "node", "data-generator.js" ]
