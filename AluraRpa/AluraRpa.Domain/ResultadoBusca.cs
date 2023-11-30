namespace AluraRpa.Domain
{
    /// <summary>
    /// Representa o resultado de uma busca.
    /// </summary>
    public class ResultadoBusca
    {
        // Propriedades da entidade ResultadoBusca

        /// <summary>
        /// Obtém ou define o título do resultado.
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Obtém ou define o(s) professor(es) associado(s) ao resultado.
        /// </summary>
        public string Professor { get; set; }

        /// <summary>
        /// Obtém ou define a carga horária relacionada ao resultado.
        /// </summary>
        public string CargaHoraria { get; set; }

        /// <summary>
        /// Obtém ou define a descrição do resultado.
        /// </summary>
        public string Descricao { get; set; }
    }
}
