import {useState} from 'react';
import {Filme} from "@/types/Filme"
import Rating from "@/components/Rating/Rating"
import Image from "next/image"
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPenToSquare, faTrash} from '@fortawesome/free-solid-svg-icons'
import ConfirmationModal from './ConfirmationModal';

interface FilmeItemProps {
  filme: Filme
  onEdit: (filme: Filme) => void
  onDelete: (id: number) => void
  onRating: (id: number, rating: number) => void
}

export default function FilmeItem({filme, onEdit, onDelete, onRating}: FilmeItemProps) {
  const [showConfirmModal, setShowConfirmModal] = useState(false);

  return (<>
    <div className={`w-52 rounded-lg overflow-visible`}>
      <div className="relative w-full h-72 overflow-visible">
        <div
          className="absolute w-full h-full transition-transform duration-300 hover:scale-110 hover:z-10 hover:translate-y-4">
          <Image
            src={'/filme.jpg'}
            alt={`Poster do filme ${filme.titulo}`}
            fill
            className="object-cover rounded-lg"
          />
          <div
            className="absolute inset-0 bg-black/70 opacity-0 hover:opacity-100 transition-opacity duration-300 p-4 flex flex-col justify-center rounded-lg">
            <h3 className="font-bold text-white mb-2 break-words">
              {filme.titulo}
            </h3>
            <p className="text-white text-sm mb-1">
              Gênero: {filme.genero}
            </p>
            <p className="text-white text-sm">
              Ano: {filme.anoLancamento}
            </p>
          </div>
        </div>
      </div>

      <div className="p-2">
        <div className="flex justify-between items-center">
          <h3 className="font-semibold text-sm line-clamp-1 break-words">
            {filme.titulo}
          </h3>
        </div>

        <div className="flex justify-between items-center mt-2">
          {filme.status ? (<Rating isEditable={false} rating={filme.nota}/>) : (
            <Rating isEditable={true} setRating={(nota) => onRating(filme.id, nota)} rating={0}/>)}
          <div className="flex gap-1.5">
            <button
              onClick={() => onEdit(filme)}
              title="Editar"
              className="cursor-pointer p-1 rounded-lg bg-[#ADB5BD] hover:bg-[#6C757D] transition"
            >
              <FontAwesomeIcon icon={faPenToSquare}/>
            </button>
            <button
              onClick={() => setShowConfirmModal(true)}
              title="Excluir"
              className="cursor-pointer p-1 rounded-lg bg-[#ADB5BD] hover:bg-[#6C757D] transition"
            >
              <FontAwesomeIcon icon={faTrash}/>
            </button>
          </div>
        </div>
      </div>
    </div>
    <ConfirmationModal
      isOpen={showConfirmModal}
      onClose={() => setShowConfirmModal(false)}
      onConfirm={() => onDelete(filme.id)}
      title="Confirmar Exclusão"
      message={`Tem certeza que deseja excluir o filme "${filme.titulo}"?`}
    />
  </>);
}