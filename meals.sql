INSERT INTO meal_entries
(
    user_id,
    entry_date,
    meal_date,
    meal_slot,
    name,
    total_calories,
    protein_grams,
    carbs_grams,
    fats_grams,
    ingredient_summary
)
VALUES
(
    3,
    CURRENT_DATE,
    CURRENT_DATE,
    'breakfast',
    'Breakfast shake',
    720,
    45,
    78,
    18,
    'Whey, oats, banana, peanut butter'
),
(
    3,
    CURRENT_DATE,
    CURRENT_DATE,
    'lunch',
    'Chicken rice bowl',
    890,
    62,
    95,
    22,
    'Chicken, rice, broccoli, sauce'
),
(
    3,
    CURRENT_DATE,
    CURRENT_DATE,
    'dinner',
    'Salmon potato plate',
    810,
    48,
    70,
    30,
    'Salmon, potatoes, vegetables'
);