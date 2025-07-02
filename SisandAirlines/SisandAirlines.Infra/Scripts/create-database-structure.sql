CREATE SCHEMA sisand_airlines AUTHORIZATION postgres;

CREATE TABLE sisand_airlines.airplane (
	id uuid NOT NULL,
	model varchar(100) NOT NULL,
	code varchar(50) NOT NULL,
	CONSTRAINT airplane_code_key UNIQUE (code),
	CONSTRAINT airplane_pkey PRIMARY KEY (id)
);

CREATE TABLE sisand_airlines.customer (
	id uuid NOT NULL,
	full_name varchar(255) NOT NULL,
	email varchar(255) NOT NULL,
	"document" varchar(14) NOT NULL,
	birth_date date NOT NULL,
	secondary_password_hash varchar(255) NOT NULL,
	create_date date NOT NULL,
	active bool NOT NULL,
	password_hash varchar(255) NOT NULL,
	CONSTRAINT customer_cpf_key UNIQUE (document),
	CONSTRAINT customer_email_key UNIQUE (email),
	CONSTRAINT customer_pkey PRIMARY KEY (id)
);

CREATE TABLE sisand_airlines.flight (
	id uuid NOT NULL,
	departure_date date NOT NULL,
	arrival_date date NOT NULL,
	duration interval NOT NULL,
	origin varchar(50) NOT NULL,
	destination varchar(50) NOT NULL,
	code varchar(50) NOT NULL,
	airplane_id uuid NOT NULL,
	start_time interval NOT NULL,
	CONSTRAINT flight_pkey PRIMARY KEY (id),
	CONSTRAINT flight_airplane_id_fkey FOREIGN KEY (airplane_id) REFERENCES sisand_airlines.airplane(id)
);

CREATE TABLE sisand_airlines.seat (
	id uuid NOT NULL,
	flight_id uuid NOT NULL,
	"number" varchar(5) NOT NULL,
	seat_type int4 NOT NULL,
	is_reserved bool NOT NULL,
	CONSTRAINT seat_flight_id_number_key UNIQUE (flight_id, number),
	CONSTRAINT seat_pkey PRIMARY KEY (id),
	CONSTRAINT seat_flight_id_fkey FOREIGN KEY (flight_id) REFERENCES sisand_airlines.flight(id)
);

CREATE TABLE sisand_airlines.shopping_cart (
	id uuid NOT NULL,
	customer_id uuid NULL,
	created_date date NOT NULL,
	is_finalized bool NOT NULL DEFAULT false,
	"session" varchar(100) NOT NULL,
	CONSTRAINT shopping_cart_pkey PRIMARY KEY (id),
	CONSTRAINT shopping_cart_customer_id_fkey FOREIGN KEY (customer_id) REFERENCES sisand_airlines.customer(id)
);

CREATE TABLE sisand_airlines.shopping_cart_item (
	id uuid NOT NULL,
	shopping_cart_id uuid NOT NULL,
	flight_id uuid NOT NULL,
	seat_type varchar(20) NOT NULL,
	quantity int4 NOT NULL,
	unit_price numeric(10, 2) NOT NULL,
	CONSTRAINT shopping_cart_item_pkey PRIMARY KEY (id),
	CONSTRAINT shopping_cart_item_flight_id_fkey FOREIGN KEY (flight_id) REFERENCES sisand_airlines.flight(id),
	CONSTRAINT shopping_cart_item_shopping_cart_id_fkey FOREIGN KEY (shopping_cart_id) REFERENCES sisand_airlines.shopping_cart(id)
);

CREATE TABLE sisand_airlines.ticket (
	id uuid NOT NULL,
	flight_id uuid NOT NULL,
	customer_id uuid NOT NULL,
	seat_id uuid NOT NULL,
	issue_date date NOT NULL,
	confirmation_code varchar(20) NOT NULL,
	CONSTRAINT ticket_pkey PRIMARY KEY (id),
	CONSTRAINT ticket_seat_id_key UNIQUE (seat_id),
	CONSTRAINT ticket_customer_id_fkey FOREIGN KEY (customer_id) REFERENCES sisand_airlines.customer(id),
	CONSTRAINT ticket_flight_id_fkey FOREIGN KEY (flight_id) REFERENCES sisand_airlines.flight(id),
	CONSTRAINT ticket_seat_id_fkey FOREIGN KEY (seat_id) REFERENCES sisand_airlines.seat(id)
);

CREATE TABLE sisand_airlines.purchase (
	id uuid NOT NULL,
	customer_id uuid NOT NULL,
	shopping_cart_id uuid NOT NULL,
	purchase_date date NULL,
	total_amount numeric(10, 2) NOT NULL,
	payment_method varchar(10) NOT NULL,
	confirmation_code varchar(20) NOT NULL,
	CONSTRAINT purchase_pkey PRIMARY KEY (id),
	CONSTRAINT purchase_customer_id_fkey FOREIGN KEY (customer_id) REFERENCES sisand_airlines.customer(id),
	CONSTRAINT purchase_shopping_cart_id_fkey FOREIGN KEY (shopping_cart_id) REFERENCES sisand_airlines.shopping_cart(id)
);
