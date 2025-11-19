CREATE TABLE users (
    id            UUID        PRIMARY KEY,
    full_name     VARCHAR(150) NOT NULL,
    email         VARCHAR(150) NOT NULL UNIQUE,
    cpf           VARCHAR(11)  NOT NULL UNIQUE,
    birth_date    DATE         NOT NULL,
    password_hash TEXT         NOT NULL,
    created_at    TIMESTAMPTZ  NOT NULL DEFAULT NOW()
);

CREATE TABLE flights (
    id              UUID        PRIMARY KEY,
    date            DATE        NOT NULL,
    departure_time  TIME        NOT NULL,
    arrival_time    TIME        NOT NULL,
    aircraft_number INT         NOT NULL  -- 1, 2 ou 3
);

--índice para buscas por data (lista de voos por dia)
CREATE INDEX idx_flights_date ON flights(date);


CREATE TABLE bookings (
    id                UUID         PRIMARY KEY,
    user_id           UUID         NOT NULL,
    flight_id         UUID         NOT NULL,
    total_amount      NUMERIC(10,2) NOT NULL,
    payment_method    VARCHAR(50)   NOT NULL,
    confirmation_code VARCHAR(50)   NOT NULL UNIQUE,
    created_at        TIMESTAMPTZ   NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_bookings_user
        FOREIGN KEY (user_id) REFERENCES users(id),
    CONSTRAINT fk_bookings_flight
        FOREIGN KEY (flight_id) REFERENCES flights(id)
);

CREATE TABLE booking_passengers (
    id           UUID         PRIMARY KEY,
    booking_id   UUID         NOT NULL,
    full_name    VARCHAR(150) NOT NULL,
    document_cpf VARCHAR(11)  NOT NULL,

    CONSTRAINT fk_booking_passengers_booking
        FOREIGN KEY (booking_id) REFERENCES bookings(id)
);

CREATE TABLE seat_reservations (
    id          UUID NOT NULL PRIMARY KEY,
    flight_id   UUID NOT NULL,
    booking_id  UUID NOT NULL,
    seat_number INT  NOT NULL,
    seat_class  INT  NOT NULL, -- 1 = Economica, 2 = Primeira Classe

    CONSTRAINT fk_seat_reservations_flight
        FOREIGN KEY (flight_id) REFERENCES flights(id),
    CONSTRAINT fk_seat_reservations_booking
        FOREIGN KEY (booking_id) REFERENCES bookings(id)
);

CREATE UNIQUE INDEX uq_seat_reservations_flight_seat
    ON seat_reservations(flight_id, seat_number);
