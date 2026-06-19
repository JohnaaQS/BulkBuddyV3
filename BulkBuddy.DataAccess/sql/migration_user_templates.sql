-- Voeg user_id en directe voedingskolommen toe aan meal_templates.
-- user_id = NULL  → global/systeem-template (is_system = true)
-- user_id = <id>  → private template van die gebruiker (is_system = false)

ALTER TABLE meal_templates
    ADD COLUMN IF NOT EXISTS user_id          INTEGER REFERENCES users(id) ON DELETE CASCADE,
    ADD COLUMN IF NOT EXISTS total_calories   INTEGER NOT NULL DEFAULT 0,
    ADD COLUMN IF NOT EXISTS protein_grams    INTEGER NOT NULL DEFAULT 0,
    ADD COLUMN IF NOT EXISTS carbs_grams      INTEGER NOT NULL DEFAULT 0,
    ADD COLUMN IF NOT EXISTS fats_grams       INTEGER NOT NULL DEFAULT 0,
    ADD COLUMN IF NOT EXISTS ingredient_summary TEXT NOT NULL DEFAULT '',
    ADD COLUMN IF NOT EXISTS created_at       TIMESTAMPTZ NOT NULL DEFAULT NOW();

-- Vul de directe voedingskolommen voor bestaande globale templates.
UPDATE meal_templates mt
SET
    total_calories    = COALESCE(sub.cal,   0),
    protein_grams     = COALESCE(sub.prot,  0),
    carbs_grams       = COALESCE(sub.carbs, 0),
    fats_grams        = COALESCE(sub.fats,  0),
    ingredient_summary = COALESCE(sub.ingr, '')
FROM (
    SELECT
        template_id,
        SUM(calories)                                     AS cal,
        SUM(protein_grams)                                AS prot,
        SUM(carbs_grams)                                  AS carbs,
        SUM(fats_grams)                                   AS fats,
        STRING_AGG(name, ', ' ORDER BY sort_order)        AS ingr
    FROM meal_template_ingredients
    GROUP BY template_id
) sub
WHERE mt.id = sub.template_id;
