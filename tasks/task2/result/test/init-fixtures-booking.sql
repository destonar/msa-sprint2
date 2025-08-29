DELETE FROM bookings where true;

-- Бронирования (для GET /api/bookings)
INSERT INTO bookings ("UserId", "HotelId", "PromoCode", "DiscountPercent", "Price", "CreatedAt")
VALUES
    ('test-user-2', 'test-hotel-1', 'TESTCODE1', 10.0, 90.0, NOW()),
    ('test-user-3', 'test-hotel-1', null, 0.0, 80.0, NOW());
