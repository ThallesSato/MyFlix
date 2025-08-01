import FilmeItem from "./FilmeItem";
import { Filme } from "@/types/Filme";

interface FilmeListProps {
  filmes: Filme[];
  onEdit: (filme: Filme) => void;
  onDelete: (id: number) => void;
  onRating: (id: number, rating: number) => void;
}

export default function FilmeList({ filmes, onEdit, onDelete, onRating }: FilmeListProps) {
  const hasNoFilmes = filmes.length === 0;

  return (
    <section className="mb-8">
      {hasNoFilmes ? (
        <div className="text-center text-gray-500 py-10 bg-[#F8F9FA] rounded-2xl shadow-sm">
          <p className="text-lg font-medium">Nenhum filme encontrado</p>
          <p className="text-sm mt-1">
            {`Tente adicionar um novo filme ou ajustar a busca.`}
          </p>
        </div>
      ) : (
        <ul className="bg-[#F8F9FA] rounded-2xl p-5 flex overflow-x-scroll scrollbar-hide lg:overflow-x-auto lg:grid lg:grid-cols-4 gap-4">
          {filmes.map((filme) => (
            <li key={filme.id}>
              <FilmeItem
                filme={filme}
                onEdit={onEdit}
                onDelete={onDelete}
                onRating={onRating}
              />
            </li>
          ))}
        </ul>
      )}
    </section>
  );
}
