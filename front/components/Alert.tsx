'use client';

import { useEffect } from 'react'

interface AlertProps {
  isOpen: boolean
  message: string
  type: 'success' | 'error'
  onClose: () => void
}

export default function Alert({ isOpen, message, type, onClose }: AlertProps) {
  useEffect(() => {
    if (isOpen) {
      const timer = setTimeout(onClose, 2000)
      return () => clearTimeout(timer)
    }
  }, [isOpen, onClose])

  const backgroundColor = type === 'success' ? 'bg-[#495057]' : 'bg-red-500'

  return (
    <div
      className={`fixed bottom-15 left-0 right-0 flex justify-center items-center transition-all duration-300 ease-in-out ${
        isOpen ? 'translate-y-0' : 'translate-y-[100px]'
      }`}
    >
      <div
        className={`${backgroundColor} text-white p-4 rounded-lg shadow-lg text-center min-w-[300px] mx-4 mb-0`}
      >
        {message}
      </div>
    </div>
  )
}