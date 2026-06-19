-- Migratie: workout_sets - snapshot vervangen door FK naar exercises
-- Verwijder exercise_name en muscle_group (worden nu via JOIN op exercises opgehaald)
-- Voeg exercise_id FK toe

BEGIN;

-- Stap 1: verwijder oude snapshot-kolommen
ALTER TABLE workout_sets
    DROP COLUMN IF EXISTS exercise_name,
    DROP COLUMN IF EXISTS muscle_group;

-- Stap 2: voeg exercise_id FK toe
ALTER TABLE workout_sets
    ADD COLUMN exercise_id INT NOT NULL REFERENCES exercises(id);

-- Index voor snellere JOINs
CREATE INDEX IF NOT EXISTS idx_workout_sets_exercise_id
    ON workout_sets(exercise_id);

COMMIT;
