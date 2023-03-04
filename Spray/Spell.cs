using System.Collections;
using ThunderRoad;
using Extensions;
using UnityEngine;

namespace Spray;
public class Spell : SpellCastCharge {
    public string itemID = "StatueSphere";
    public float itemThrowForce = 50.0f;
    public float itemDespawnTimer = 1.50f;
    public float itemSpawnDistanceAboveHand = 0.80f;
    public float spawnDelay = 0.25f;
    public override void UpdateCaster() {
        if (!spellCaster.isFiring) return;
        GameManager.local.StartCoroutine(Spawn());
    }
    private IEnumerator Spawn() {
        while (spellCaster.isFiring) {
            Catalog.GetData<ItemData>(itemID).SpawnAsync(item => {
                item.transform.position = spellCaster.ragdollHand.DorsalHandPosition(itemSpawnDistanceAboveHand);
                item.transform.rotation = spellCaster.ragdollHand.HandRotation();
                item.rb.AddForce(-spellCaster.ragdollHand.transform.right *
                                 itemThrowForce,
                                 ForceMode.VelocityChange);
                item.Despawn(itemDespawnTimer);
                item.IgnoreRagdollCollision(Player.currentCreature.ragdoll);
            });
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}