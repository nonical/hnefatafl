using Tags;
using UnityEngine;

public class ParticlesHandler : MonoBehaviour {
    public GameObject AttackerDeathParticle;
    public GameObject DefenderDeathParticle;
    public GameObject KingDeathParticle;

    public GameObject HavenTileParticle;

    public void DeathParticleEffect(GameObject figure) {
        GameObject particle = null;
        if (figure.CompareTag(FigureTags.TeamA)) particle = Instantiate(AttackerDeathParticle);
        if (figure.CompareTag(FigureTags.TeamB)) particle = Instantiate(DefenderDeathParticle);
        if (figure.CompareTag(FigureTags.King)) particle = Instantiate(KingDeathParticle);

        particle.transform.position = figure.transform.position;
    }

    public void KingEscapeEffect((int i, int j) havenTileIndex) {
        var king = GameMemory.Figures[havenTileIndex.i, havenTileIndex.j];

        Instantiate(HavenTileParticle, king.transform);
    }
}
