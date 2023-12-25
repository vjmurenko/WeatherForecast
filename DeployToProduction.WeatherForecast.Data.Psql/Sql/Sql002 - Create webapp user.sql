begin;

CREATE user webapp with ENCRYPTED PASSWORD 'webappPASSWORD';

grant select, update, insert on table weather to webapp;
end;
