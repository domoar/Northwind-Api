CREATE ROLE northwind LOGIN PASSWORD 'p';
CREATE DATABASE northwind;
GRANT ALL ON DATABASE northwind TO northwind;
CREATE TABLESPACE northwindspace OWNER northwind LOCATION '/var/lib/postgresql/tablespace/northwind';